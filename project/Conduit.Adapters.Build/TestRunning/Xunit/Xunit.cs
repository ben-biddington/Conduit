﻿using System;
using System.Threading;
using Xunit.Runners;

namespace Conduit.Adapters.Build.TestRunning.Xunit
{
    public class XunitOptions
    {
        public static readonly XunitOptions NoAppDomain = new XunitOptions(false); 
        public bool RunInSeparateAppDomain { get; private set; }

        public XunitOptions(bool runInSeparateAppDomain = true)
        {
            RunInSeparateAppDomain = runInSeparateAppDomain;
        }
    }

    public static class Xunit
    {
        public static bool Run(TestReport report, string testAssembly, string testClassName = null)
        {
            return Run(report, testAssembly, testClassName, new XunitOptions(true));
        }

        //private static readonly Func<string,AssemblyRunner> _on = testAssembly => AssemblyRunner.WithAppDomain(testAssembly);
        private static readonly Func<string,AssemblyRunner> _off = testAssembly => AssemblyRunner.WithoutAppDomain(testAssembly);

        public static bool Run(TestReport report, string testAssembly, string testClassName, XunitOptions options)
        {
            var result = 0;

            var finished = new ManualResetEvent(false);

            using (var runner = Choose(options)(testAssembly))
            {
                Listen(report, testAssembly, runner, finished);

                runner.Start(testClassName, parallel: false);

                runner.OnTestFailed += _ => result = 1;

                finished.WaitOne();
                finished.Dispose();

                Wait.Until(() => runner.Status.Equals(AssemblyRunnerStatus.Idle));

                return result == 0;
            }
        }

        private static Func<string,AssemblyRunner> Choose(XunitOptions opts) 
        {
            if (opts.RunInSeparateAppDomain)
                return _off;

            return _off;
        }

        private static void Listen(TestReport report, string testAssembly, AssemblyRunner runner, ManualResetEvent finished)
        {
            runner.OnDiscoveryComplete  += info => report.RunStarted(new TestRun(testAssembly, info.TestCasesToRun));
            runner.OnTestOutput         += info => report.Output(info.Output);
            runner.OnTestPassed         += info => report.Passed(new TestName(info.TestCollectionDisplayName, info.TestDisplayName));
            runner.OnTestFailed         += info =>
            {
                report.Failed(new TestFailure(info.ExceptionMessage, info.ExceptionStackTrace, new TestName(info.TestCollectionDisplayName, info.TestDisplayName)));
            };
            runner.OnTestSkipped        += info => report.Skipped(info.SkipReason, new TestName(info.TestCollectionDisplayName, info.TestDisplayName));
            runner.OnTestFinished       += info => report.Finished();
            runner.OnExecutionComplete  += info =>
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
