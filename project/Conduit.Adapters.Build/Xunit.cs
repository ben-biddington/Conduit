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

            using (var runner = AssemblyRunner.WithAppDomain(testAssembly))
            {
                runner.OnDiscoveryComplete  = info => report.RunStarted(new TestRun(testAssembly, info.TestCasesToRun));
                runner.OnTestOutput         = info => report.Output(info.Output);
                runner.OnTestPassed         = info => report.Passed(info.Output);
                runner.OnTestFailed         = info => { result = 1; report.Log(info.ExceptionMessage); report.Log(info.ExceptionStackTrace); };
                runner.OnTestSkipped        = info => report.Skipped(info.SkipReason);
                runner.OnTestFinished       = info => report.Finished(info.Output);
                runner.OnExecutionComplete  = info => { finished.Set(); report.RunFinished(new TestResult(info.TotalTests-info.TestsFailed, info.TestsFailed, info.TestsSkipped, TimeSpan.FromSeconds(Convert.ToDouble(info.ExecutionTime)))); };

                runner.Start(testClassName, parallel: false);

                finished.WaitOne();
                finished.Dispose();

                report.Log("Tests finished with status <" + result + ">");

                Wait.Until(() => runner.Status.Equals(AssemblyRunnerStatus.Idle));

                return result == 0;
            }
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

