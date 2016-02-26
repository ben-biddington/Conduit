using System;
using Xunit.Runners;
using System.Threading;

namespace Conduit.Adapters.Build
{
    public static class Xunit
    {
        public static bool Run(TestReport report, string testAssembly, string testClassName = null)
        {
            var result = 0;

            var finished = new ManualResetEvent(false);

            using (var runner = AssemblyRunner.WithoutAppDomain(testAssembly))
            {
                Listen(report, testAssembly, runner, finished);

                runner.Start(testClassName, parallel: false);

                runner.OnTestFailed = _ => result = 1;

                finished.WaitOne();
                finished.Dispose();

                Wait.Until(() => runner.Status.Equals(AssemblyRunnerStatus.Idle));

                return result == 0;
            }
        }

        private static void Listen(TestReport report, string testAssembly, AssemblyRunner runner, ManualResetEvent finished)
        {
            runner.OnDiscoveryComplete  = info => report.RunStarted(new TestRun(testAssembly, info.TestCasesToRun));
            runner.OnTestOutput         = info => report.Output(info.Output);
            runner.OnTestPassed         = info => report.Passed(info.Output);
            runner.OnTestFailed         = info =>
            {
                // Why is this not triggered?
                report.Failed(new TestFailure(info.ExceptionMessage, info.ExceptionStackTrace));
            };
            runner.OnTestSkipped        = info => report.Skipped(info.SkipReason);
            runner.OnTestFinished       = info => report.Finished();
            runner.OnExecutionComplete  = info =>
            {
                finished.Set();

                report.RunFinished(
                    new TestResult(
                        info.TotalTests - (info.TestsFailed + info.TestsSkipped), 
                        info.TestsFailed, 
                        info.TestsSkipped,
                        TimeSpan.FromSeconds(Convert.ToDouble(info.ExecutionTime))));
            };
        }
    }

    internal static class Wait
    {
        internal static void Until(Func<bool> block)
        {
            var startedAt = DateTime.Now;

            while (false == block())
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(500));

                if (DateTime.Now.Subtract(startedAt) > TimeSpan.FromSeconds(5))
                    throw new Exception($"Timed out after <{DateTime.Now.Subtract(startedAt)}>");
            }
        }
    }
}

