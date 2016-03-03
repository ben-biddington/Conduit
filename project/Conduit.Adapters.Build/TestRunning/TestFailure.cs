namespace Conduit.Adapters.Build.TestRunning
{
    public class TestFailure
    {
        public string Message { get; private set; }
        public string Stacktrace { get; private set; }
        public TestName TestName { get; private set; }

        public TestFailure(string message, string stacktrace, TestName name)
        {
            Message = message;
            Stacktrace = stacktrace;
            TestName = name;
        }
    }
}