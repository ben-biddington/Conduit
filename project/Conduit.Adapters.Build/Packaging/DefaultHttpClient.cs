using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using NuGet;

namespace Conduit.Adapters.Build.Packaging
{
    /// <summary>
    /// https://raw.githubusercontent.com/NuGet/NuGet2/2.13/src/Core/Http/HttpClient.cs
    /// </summary>
    public class DefaultHttpClient : IHttpClient
    {
        public class Options
        {
            public static Options Default = new Options(true);

            public bool BypassProxy { get; }
            public Action<string> Log { get; private set; }

            public Options(bool bypassProxy)
            {
                BypassProxy = bypassProxy;
                Log = _ => { };
            }

            public Options With(Action<string> log)
            {
                return new Options(BypassProxy) { Log = m => { log($"[{typeof(DefaultHttpClient).FullName}] {m}"); } };
            }
        }

        public event EventHandler<ProgressEventArgs> ProgressAvailable = delegate { };
        public event EventHandler<WebRequestEventArgs> SendingRequest = delegate { };

        private static ICredentialProvider _credentialProvider;
        private Uri _uri;
        private readonly Options _opts;

        public DefaultHttpClient(Uri uri, Options opts)
        {
            _uri = uri;
            _opts = opts;
        }

        public string UserAgent
        {
            get;
            set;
        }

        public virtual Uri Uri
        {
            get
            {
                return _uri;
            }
            set
            {
                _uri = value;
            }
        }

        public virtual Uri OriginalUri
        {
            get { return _uri; }
        }

        public string Method
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public bool AcceptCompression
        {
            get;
            set;
        }

        public bool DisableBuffering
        {
            get;
            set;
        }

        // TODO: Get rid of this. Horrid to have static properties like this especially in a code path that does not look thread safe.
        public static ICredentialProvider DefaultCredentialProvider
        {
            get
            {
                return _credentialProvider ?? NullCredentialProvider.Instance;
            }
            set
            {
                _credentialProvider = value;
            }
        }

        public virtual WebResponse GetResponse()
        {
            Func<WebRequest> webRequestFactory = () =>
            {
                WebRequest request = WebRequest.Create(Uri);
                InitializeRequestProperties(request);
                return request;
            };

            var requestHelper = new RequestHelper(
                webRequestFactory,
                RaiseSendingRequest,
                ProxyCache.Instance,
                CredentialStore.Instance,
                DefaultCredentialProvider,
                DisableBuffering);

            return requestHelper.GetResponse();
        }

        public void InitializeRequest(WebRequest request)
        {
            InitializeRequestProperties(request);

            if (!_opts.BypassProxy)
            {
                _opts.Log("Bypassing proxy");
                request.Proxy = null;
                request.Credentials = null;
            }
            else
            {
                TrySetCredentialsAndProxy(request);
            }

            RaiseSendingRequest(request);
        }

        private void TrySetCredentialsAndProxy(WebRequest request)
        {
            // Used the cached credentials and proxy we have
            request.Credentials = CredentialStore.Instance.GetCredentials(Uri);
            request.Proxy = ProxyCache.Instance.GetProxy(Uri);
            STSAuthHelper.PrepareSTSRequest(request);
        }

        private void InitializeRequestProperties(WebRequest request)
        {
            var httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                httpRequest.UserAgent = UserAgent ?? HttpUtility.CreateUserAgentString("NuGet");
                httpRequest.CookieContainer = new CookieContainer();
                if (AcceptCompression)
                {
                    httpRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }
            }

            if (!String.IsNullOrEmpty(ContentType))
            {
                request.ContentType = ContentType;
            }

            if (!String.IsNullOrEmpty(Method))
            {
                request.Method = Method;
            }
        }

        public void DownloadData(Stream targetStream)
        {
            const int ChunkSize = 1024 * 4; // 4KB

            using (var response = GetResponse())
            {
                // Total response length
                int length = (int)response.ContentLength;
                using (Stream stream = response.GetResponseStream())
                {
                    // in some circumstances, the Content-Length response header is missing, resulting in
                    // the ContentLength = -1. In which case, we copy the whole stream and do not report progress.
                    if (length < 0)
                    {
                        stream.CopyTo(targetStream);

                        // reporting fake progress as 100%
                        OnProgressAvailable(100);
                    }
                    else
                    {
                        // We read the response stream chunk by chunk (each chunk is 4KB). 
                        // After reading each chunk, we report the progress based on the total number bytes read so far.
                        int totalReadSoFar = 0;
                        byte[] buffer = new byte[ChunkSize];
                        while (totalReadSoFar < length)
                        {
                            int bytesRead = stream.Read(buffer, 0, Math.Min(length - totalReadSoFar, ChunkSize));
                            if (bytesRead == 0)
                            {
                                break;
                            }
                            else
                            {
                                targetStream.Write(buffer, 0, bytesRead);

                                totalReadSoFar += bytesRead;
                                OnProgressAvailable((int)(((long)totalReadSoFar * 100) / length)); // avoid 32 bit overflow by calculating * 100 with 64 bit
                            }
                        }
                    }
                }
            }
        }

        private void OnProgressAvailable(int percentage)
        {
            ProgressAvailable(this, new ProgressEventArgs(percentage));
        }

        private void RaiseSendingRequest(WebRequest webRequest)
        {
            SendingRequest(this, new WebRequestEventArgs(webRequest));
        }
    }

    /// <summary>
    /// This class is used to keep sending requests until a response code that doesn't require 
    /// authentication happens or if the request requires authentication and 
    /// the user has stopped trying to enter them (i.e. they hit cancel when they are prompted).
    /// </summary>
    internal class RequestHelper
    {
        private Func<WebRequest> _createRequest;
        private Action<WebRequest> _prepareRequest;
        private IProxyCache _proxyCache;
        private ICredentialCache _credentialCache;
        private ICredentialProvider _credentialProvider;

        HttpWebRequest _previousRequest;
        IHttpWebResponse _previousResponse;
        HttpStatusCode? _previousStatusCode;
        int _credentialsRetryCount;
        bool _usingSTSAuth;
        bool _continueIfFailed;
        int _proxyCredentialsRetryCount;
        bool _basicAuthIsUsedInPreviousRequest;
        bool _disableBuffering;

        public RequestHelper(Func<WebRequest> createRequest,
            Action<WebRequest> prepareRequest,
            IProxyCache proxyCache,
            ICredentialCache credentialCache,
            ICredentialProvider credentialProvider,
            bool disableBuffering)
        {
            _createRequest = createRequest;
            _prepareRequest = prepareRequest;
            _proxyCache = proxyCache;
            _credentialCache = credentialCache;
            _credentialProvider = credentialProvider;
            _disableBuffering = disableBuffering;
        }

        public WebResponse GetResponse()
        {
            _previousRequest = null;
            _previousResponse = null;
            _previousStatusCode = null;
            _usingSTSAuth = false;
            _continueIfFailed = true;
            _proxyCredentialsRetryCount = 0;
            _credentialsRetryCount = 0;
            int failureCount = 0;
            const int MaxFailureCount = 10;

            while (true)
            {
                // Create the request
                var request = (HttpWebRequest)_createRequest();
                ConfigureRequest(request);

                try
                {
                    if (_disableBuffering)
                    {
                        request.AllowWriteStreamBuffering = false;

                        // When buffering is disabled, we need to add the Authorization header 
                        // for basic authentication by ourselves.
                        bool basicAuth = _previousResponse != null &&
                                         _previousResponse.AuthType != null &&
                                         _previousResponse.AuthType.IndexOf("Basic", StringComparison.OrdinalIgnoreCase) != -1;
                        var networkCredentials = request.Credentials.GetCredential(request.RequestUri, "Basic");
                        if (networkCredentials != null && basicAuth)
                        {
                            string authInfo = networkCredentials.UserName + ":" + networkCredentials.Password;
                            authInfo = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(authInfo));
                            request.Headers["Authorization"] = "Basic " + authInfo;
                            _basicAuthIsUsedInPreviousRequest = true;
                        }
                    }

                    // Prepare the request, we do something like write to the request stream
                    // which needs to happen last before the request goes out
                    _prepareRequest(request);

                    WebResponse response = request.GetResponse();

                    // Cache the proxy and credentials
                    _proxyCache.Add(request.Proxy);

                    ICredentials credentials = request.Credentials;
                    _credentialCache.Add(request.RequestUri, credentials);
                    _credentialCache.Add(response.ResponseUri, credentials);

                    return response;
                }
                catch (WebException ex)
                {
                    ++failureCount;
                    if (failureCount >= MaxFailureCount)
                    {
                        throw;
                    }

                    using (IHttpWebResponse response = GetResponse(ex.Response))
                    {
                        if (response == null &&
                            ex.Status != WebExceptionStatus.SecureChannelFailure)
                        {
                            // No response, something went wrong so just rethrow
                            throw;
                        }

                        // Special case https connections that might require authentication
                        if (ex.Status == WebExceptionStatus.SecureChannelFailure)
                        {
                            if (_continueIfFailed)
                            {
                                SetPreviousStatusCode(request, ex);
                                continue;
                            }
                            throw;
                        }

                        // If we were trying to authenticate the proxy or the request and succeeded, cache the result.
                        if (_previousStatusCode == HttpStatusCode.ProxyAuthenticationRequired &&
                            response.StatusCode != HttpStatusCode.ProxyAuthenticationRequired)
                        {
                            _proxyCache.Add(request.Proxy);
                        }
                        else if (_previousStatusCode == HttpStatusCode.Unauthorized &&
                                 response.StatusCode != HttpStatusCode.Unauthorized)
                        {
                            _credentialCache.Add(request.RequestUri, request.Credentials);
                            _credentialCache.Add(response.ResponseUri, request.Credentials);
                        }

                        _usingSTSAuth = STSAuthHelper.TryRetrieveSTSToken(request.RequestUri, response);

                        if (!IsAuthenticationResponse(response) || !_continueIfFailed)
                        {
                            throw;
                        }

                        if (!EnvironmentUtility.IsNet45Installed &&
                            request.AllowWriteStreamBuffering == false &&
                            response.AuthType != null &&
                            IsNtlmOrKerberos(response.AuthType))
                        {
                            // integrated windows authentication does not work when buffering is 
                            // disabled on .net 4.0.
                            throw;
                        }

                        _previousRequest = request;
                        _previousResponse = response;
                        _previousStatusCode = _previousResponse.StatusCode;
                    }
                }
            }
        }

        private void SetPreviousStatusCode(HttpWebRequest request, WebException ex)
        {
            // HACK!!! : This is a hack to workaround Xamarin Bug 19594
            if (IsMonoProxyAuthenticationRequired(request, ex))
            {
                _previousStatusCode = HttpStatusCode.ProxyAuthenticationRequired;
            }
            else
            {
                // Act like we got a 401 so that we prompt for credentials on the next request
                _previousStatusCode = HttpStatusCode.Unauthorized;
            }
        }

        private static bool IsMonoProxyAuthenticationRequired(HttpWebRequest request, WebException ex)
        {
            return EnvironmentUtility.IsMonoRuntime &&
                   request.Proxy != null &&
                   ex.Message != null &&
                   ex.Message.Contains("The remote server returned a 407 status code.");
        }

        private void ConfigureRequest(HttpWebRequest request)
        {
            request.Proxy = _proxyCache.GetProxy(request.RequestUri);
            if (request.Proxy != null && request.Proxy.Credentials == null)
            {
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            if (_previousResponse == null || ShouldKeepAliveBeUsedInRequest(_previousRequest, _previousResponse))
            {
                if (_previousStatusCode == HttpStatusCode.ProxyAuthenticationRequired && _previousResponse == null &&
                    EnvironmentUtility.IsMonoRuntime)
                {
                    GetProxyCredentials(request);
                }
                else
                {
                    // Try to use the cached credentials (if any, for the first request)
                    request.Credentials = _credentialCache.GetCredentials(request.RequestUri);

                    // If there are no cached credentials, use the default ones
                    if (request.Credentials == null)
                    {
                        request.UseDefaultCredentials = true;
                    }
                }

            }
            else if (_previousStatusCode == HttpStatusCode.ProxyAuthenticationRequired)
            {
                GetProxyCredentials(request);
            }
            else if (_previousStatusCode == HttpStatusCode.Unauthorized)
            {
                SetCredentialsOnAuthorizationError(request);
            }

            SetKeepAliveHeaders(request, _previousResponse);
            if (_usingSTSAuth)
            {
                // Add request headers if the server requires STS based auth.
                STSAuthHelper.PrepareSTSRequest(request);
            }

            // Wrap the credentials in a CredentialCache in case there is a redirect
            // and credentials need to be kept around.
            request.Credentials = request.Credentials.AsCredentialCache(request.RequestUri);
        }

        private void GetProxyCredentials(HttpWebRequest request)
        {
            request.Proxy.Credentials = _credentialProvider.GetCredentials(
                request, CredentialType.ProxyCredentials, retrying: _proxyCredentialsRetryCount > 0);
            _continueIfFailed = request.Proxy.Credentials != null;
            _proxyCredentialsRetryCount++;
        }

        private void SetCredentialsOnAuthorizationError(HttpWebRequest request)
        {
            if (_usingSTSAuth)
            {
                // If we are using STS, the auth's being performed by a request header. 
                // We do not need to ask the user for credentials at this point.
                return;
            }

            // When buffering is disabled, we need to handle basic auth ourselves.
            bool basicAuth = _previousResponse.AuthType != null &&
                             _previousResponse.AuthType.IndexOf("Basic", StringComparison.OrdinalIgnoreCase) != -1;
            if (_disableBuffering && basicAuth && !_basicAuthIsUsedInPreviousRequest)
            {
                // The basic auth credentials were not sent in the last request. 
                // We need to try with cached credentials in this request.        
                request.Credentials = _credentialCache.GetCredentials(request.RequestUri);
            }

            if (request.Credentials == null)
            {
                request.Credentials = _credentialProvider.GetCredentials(
                    request, CredentialType.RequestCredentials, retrying: _credentialsRetryCount > 0);
            }

            _continueIfFailed = request.Credentials != null;
            _credentialsRetryCount++;
        }

        private static IHttpWebResponse GetResponse(WebResponse response)
        {
            var httpWebResponse = response as IHttpWebResponse;
            if (httpWebResponse == null)
            {
                var webResponse = response as HttpWebResponse;
                if (webResponse == null)
                {
                    return null;
                }
                return new HttpWebResponseWrapper(webResponse);
            }

            return httpWebResponse;
        }

        private static bool IsAuthenticationResponse(IHttpWebResponse response)
        {
            return response.StatusCode == HttpStatusCode.Unauthorized ||
                   response.StatusCode == HttpStatusCode.ProxyAuthenticationRequired;
        }

        private static void SetKeepAliveHeaders(HttpWebRequest request, IHttpWebResponse previousResponse)
        {
            // KeepAlive is required for NTLM and Kerberos authentication. If we've never been authenticated or are using a different auth, we 
            // should not require KeepAlive.
            // REVIEW: The WWW-Authenticate header is tricky to parse so a Equals might not be correct. 
            if (previousResponse == null ||
                !IsNtlmOrKerberos(previousResponse.AuthType))
            {
                // This is to work around the "The underlying connection was closed: An unexpected error occurred on a receive."
                // exception.
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
            }
        }

        private static bool ShouldKeepAliveBeUsedInRequest(HttpWebRequest request, IHttpWebResponse response)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            return !request.KeepAlive && IsNtlmOrKerberos(response.AuthType);
        }

        private static bool IsNtlmOrKerberos(string authType)
        {
            if (String.IsNullOrEmpty(authType))
            {
                return false;
            }

            return authType.IndexOf("NTLM", StringComparison.OrdinalIgnoreCase) != -1
                   || authType.IndexOf("Kerberos", StringComparison.OrdinalIgnoreCase) != -1;
        }

        private class HttpWebResponseWrapper : IHttpWebResponse
        {
            private readonly HttpWebResponse _response;
            public HttpWebResponseWrapper(HttpWebResponse response)
            {
                _response = response;
            }

            public string AuthType
            {
                get
                {
                    return _response.Headers[HttpResponseHeader.WwwAuthenticate];
                }
            }

            public HttpStatusCode StatusCode
            {
                get
                {
                    return _response.StatusCode;
                }
            }

            public Uri ResponseUri
            {
                get
                {
                    return _response.ResponseUri;
                }
            }

            public NameValueCollection Headers
            {
                get
                {
                    return _response.Headers;
                }
            }

            public void Dispose()
            {
                if (_response != null)
                {
                    _response.Close();
                }
            }
        }
    }

    /// <summary>
    /// https://raw.githubusercontent.com/NuGet/NuGet2/e89c4b2fc6dc2fc752d2fceecd2a2175b0f4ab69/src/Core/Extensions/CredentialProviderExtensions.cs
    /// </summary>
    internal static class CredentialProviderExtensions
    {
        private static readonly string[] _authenticationSchemes = new[] { "Basic", "NTLM", "Negotiate" };

        internal static ICredentials GetCredentials(this ICredentialProvider provider, WebRequest request, CredentialType credentialType, bool retrying = false)
        {
            return provider.GetCredentials(request.RequestUri, request.Proxy, credentialType, retrying);
        }

        internal static ICredentials AsCredentialCache(this ICredentials credentials, Uri uri)
        {
            // No credentials then bail
            if (credentials == null)
            {
                return null;
            }

            // Do nothing with default credentials
            if (credentials == CredentialCache.DefaultCredentials ||
                credentials == CredentialCache.DefaultNetworkCredentials)
            {
                return credentials;
            }

            // If this isn't a NetworkCredential then leave it alone
            var networkCredentials = credentials as NetworkCredential;
            if (networkCredentials == null)
            {
                return credentials;
            }

            // Set this up for each authentication scheme we support
            // The reason we're using a credential cache is so that the HttpWebRequest will forward our
            // credentials if there happened to be any redirects in the chain of requests.
            var cache = new CredentialCache();
            foreach (var scheme in _authenticationSchemes)
            {
                cache.Add(uri, scheme, networkCredentials);
            }
            return cache;
        }
    }
}
