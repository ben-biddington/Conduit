using System;
using Conduit.Lang;

namespace Conduit.Adapters.Build
{
    public class TestReport
    {
        public static TestReport Silent = new TestReport();
        public static TestReport Normal(Action<string> log) => Silent.With(it =>
        {
            it.Log      = log;
            it.Passed   = log;
            it.Finished = log;
        });

        private readonly static Action<string> noop = _ => { };

        public Action<string> Failed    { get; private set; }
        public Action<string> Skipped   { get; private set; }
        public Action<string> Finished  { get; private set; }
        public Action<string> Output    { get; private set; }
        public Action<string> Passed    { get; private set; }
        public Action<string> Log       { get; private set; }

        public TestReport() : this(noop, noop, noop, noop, noop, noop)
        {
            
        }

        public TestReport(
            Action<string> output, 
            Action<string> finished, 
            Action<string> passed, 
            Action<string> failed, 
            Action<string> skipped, 
            Action<string> log)
        {
            Output      = output;
            Finished    = finished;
            Passed      = passed;
            Failed      = failed;
            Skipped     = skipped;
            Log         = log;
        }

        public TestReport With(Action<TestReport> block)
        {
            return ((TestReport)MemberwiseClone()).Tap(block);
        }
    }
}