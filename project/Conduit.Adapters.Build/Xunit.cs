using System.Linq;
using System;
using System.Collections;
using Xunit.Runners;
using System.Threading;

namespace Conduit.Adapters.Build
{
	public static class Xunit
	{
		public static bool Run(Action<string> log, string testAssembly, string testClassName=null)
		{
			// Is there an Xunit sdk for running tests programmatically?
			// See: https://github.com/xunit/samples.xunit/blob/master/TestRunner/Program.cs

			var result = 0;

			ManualResetEvent finished = new ManualResetEvent(false);

			using (var runner = AssemblyRunner.WithAppDomain(testAssembly))
			{
				runner.OnDiscoveryComplete 	= info => log("Running <" + info.TestCasesToRun + "> of <" + info.TestCasesDiscovered + "> tests found");
				runner.OnExecutionComplete 	= info => { finished.Set(); log("Passed: " + (info.TotalTests - (info.TestsFailed + info.TestsSkipped))); };
				runner.OnTestFailed 		= info => { result = 1;  log(info.ExceptionMessage); };
				runner.OnTestOutput			= info => log(info.Output);
				runner.OnTestPassed 		= info => log(info.Output);
				runner.OnTestFinished 		= _ => log(".");
				runner.OnTestSkipped 		= _ => log("*");

				log("Discovering tests in assembly <" +  testAssembly + ">");

				runner.Start(testClassName, parallel: false);

				finished.WaitOne();
				finished.Dispose();

				log ("Tests finished with status <" + result + ">");

				return result == 0;
			}
		}
	}
}

