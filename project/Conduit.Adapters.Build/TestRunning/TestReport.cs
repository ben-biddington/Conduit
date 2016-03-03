using System;
using Conduit.Lang;

namespace Conduit.Adapters.Build.TestRunning
{
    // @todo: this may need to be an object because it needs to remember the things it is told, that way we can report failures at the end
    public class TestReport
    {
        private static readonly Action<string> noop = Noop<string>();

        public static TestReport Silent = new TestReport();

        public static TestReport Normal(Action<string> log) => Silent.With(it =>
        {
            it.Log          = log;
            it.Passed       = _ => Console.Write("."); // @todo: this needs to be formatted indented like the rest of the text
            it.Failed       = _ => Console.Write("F");
            it.Skipped      = (_,__) => Console.Write("*");
            it.RunStarted   = info => log($"Running <{info.TestCaseCount}> tests from assembly <{info.AssemblyName}>");
            it.RunFinished  = info =>
            {
                Console.WriteLine(Environment.NewLine);
                log($"passed: {info.Passed}, failed: {info.Failed}, skipped: {info.Skipped}, duration: {info.Duration}");
            };
        });
        
        public static TestReport Documentation(Action<string> log) => Normal(log).With(it =>
        {
            it.Passed       = info          => log($"[PASSED] {info.Name}");
            it.Failed       = info          => log($"[FAILED] {info.TestName.Name} ({info.Message})");
            it.Skipped      = (reason,name) => log($"[SKIPPED, {reason}] {name.Name}");
        });

        private static Action<T> Noop<T>()
        {
            return _ => { };
        }

        public Action<TestFailure>      Failed      { get; private set; }
        public Action<string, TestName> Skipped     { get; private set; }
        public Action<TestResult>       RunFinished { get; private set; }
        public Action                   Finished    { get; private set; }
        public Action<string>           Output      { get; private set; }
        public Action<TestRun>          RunStarted  { get; private set; }
        public Action<TestName>         Passed      { get; private set; }
        public Action<string>           Log         { get; private set; }

        public TestReport() : this(noop, Noop<TestRun>(), () => { }, Noop<TestName>(), Noop<TestFailure>(), (_,__) => {}, Noop<TestResult>(), noop) { }

        public TestReport(
            Action<string>          output, 
            Action<TestRun>         runStarted, 
            Action                  finished, 
            Action<TestName>        passed, 
            Action<TestFailure>     failed, 
            Action<string,TestName> skipped, 
            Action<TestResult>      runFinished, 
            Action<string>          log)
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

        public TestReport WithFailure(Action<TestFailure> block)
        {
            return With(it => it.Failed = block);
        }

        public TestReport With(Action<TestReport> block)
        {
            return ((TestReport)MemberwiseClone()).Tap(block);
        }
    }
}