using Microsoft.Build.Framework;

namespace Conduit.Build.Targets
{
    internal static class Cli
    {
        internal static void Say(IBuildEngine engine, string format, params object[] args)
        {
            engine.LogMessageEvent(new BuildMessageEventArgs(string.Format((format ?? string.Empty), args), string.Empty, string.Empty, MessageImportance.Normal));
        }
    }
}