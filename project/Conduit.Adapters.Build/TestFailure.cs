namespace Conduit.Adapters.Build
{
    public class TestFailure
    {
        public string Message { get; private set; }
        public string Stacktrace { get; private set; }

        public TestFailure(string message, string stacktrace)
        {
            Message = message;
            Stacktrace = stacktrace;
        }
    }
}