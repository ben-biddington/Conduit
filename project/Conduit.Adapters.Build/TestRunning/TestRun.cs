namespace Conduit.Adapters.Build.TestRunning
{
    public class TestRun
    {
        public string AssemblyName { get; set; }
        public int TestCaseCount { get; private set; }

        public TestRun(string assemblyName, int testCaseCount)
        {
            AssemblyName = assemblyName;
            TestCaseCount = testCaseCount;
        }
    }
}