using System;
using Conduit.Lang;

namespace Conduit.Adapters.Build
{
    public class TestReport
    {
        private static readonly Action<string> noop = Noop<string>();

        public static TestReport Silent = new TestReport();
        public static TestReport Normal(Action<string> log) => Silent.With(it =>
        {
            it.Log          = log;
            it.Passed       = log;
            it.Finished     = log;
            it.RunStarted   = info => log($"Running <{info.TestCaseCount}> tests from assembly <{info.AssemblyName}>");
            it.RunFinished  = info => log($"passed: {info.Passed}, failed: {info.Failed}, skipped: {info.Skipped}, duration: {info.Duration}");
        });

        private static Action<T> Noop<T>()
        {
            return _ => { };
        }

        public Action<string> Failed            { get; private set; }
        public Action<string> Skipped           { get; private set; }
        public Action<TestResult> RunFinished   { get; set; }
        public Action<string> Finished          { get; private set; }
        public Action<string> Output            { get; private set; }
        public Action<TestRun> RunStarted       { get; set; }
        public Action<string> Passed            { get; private set; }
        public Action<string> Log               { get; private set; }

        public TestReport() : this(noop, Noop<TestRun>(), noop, noop, noop, noop, Noop<TestResult>(), noop)
        {
            
        }

        public TestReport(
            Action<string>      output, 
            Action<TestRun>     runStarted, 
            Action<string>      finished, 
            Action<string>      passed, 
            Action<string>      failed, 
            Action<string>      skipped, 
            Action<TestResult>  runFinished, 
            Action<string>      log)
        {
            Output      = output;
            RunStarted  = runStarted;
            Finished    = finished;
            Passed      = passed;
            Failed      = failed;
            Skipped     = skipped;
            RunFinished = runFinished;
            Log         = log;
        }

        public TestReport With(Action<TestReport> block)
        {
            return ((TestReport)MemberwiseClone()).Tap(block);
        }
    }
}