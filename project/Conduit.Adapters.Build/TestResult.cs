using System;

namespace Conduit.Adapters.Build
{
    public class TestResult
    {
        public int Passed { get; }
        public int Failed { get; }
        public int Skipped { get; }
        public TimeSpan Duration { get; }

        public TestResult(int passed, int failed, int skipped, TimeSpan duration)
        {
            Passed = passed;
            Failed = failed;
            Skipped = skipped;
            Duration = duration;
        }
    }

    public class TestName 
    {
        public string Collection { get; }
        public string Name { get; }

        public TestName(string collection, string name)
        {
            Collection = collection;
            Name = name;
        }
    }
}