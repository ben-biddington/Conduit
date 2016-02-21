using System.Linq;
using System;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Collections;
using Xunit.Runners;
using System.Threading;

namespace Conduit.Build.Targets
{
	public class Xunit : Task
	{
		public String TestAssembly { get; set;}

		/// <summary>
		/// xbuild path/to/name.csproj /property:TestAssembly=path/to/AssemblyName.dll /t:T
		/// </summary>
		public override bool Execute()
		{
			// Is there an Xunit sdk for running tests programmatically?
			// See: https://github.com/xunit/samples.xunit/blob/master/TestRunner/Program.cs

			Cli.Say(BuildEngine, "Running test assembly <{0}>", TestAssembly);

			Action<string> say = msg => Cli.Say(BuildEngine, msg);

			string typeName = null; // https://github.com/xunit/xunit/blob/master/src/xunit.runner.utility/Runners/AssemblyRunner.cs#L173

			var result = 0;

			ManualResetEvent finished = new ManualResetEvent(false);

			using (var runner = AssemblyRunner.WithAppDomain(TestAssembly))
			{
				runner.OnDiscoveryComplete 	= info => say("Running <" + info.TestCasesToRun + "> of <" + info.TestCasesDiscovered + "> tests found");
				runner.OnExecutionComplete 	= info => { finished.Set(); say("Passed: " + (info.TotalTests - (info.TestsFailed + info.TestsSkipped))); };
				runner.OnTestFailed 		= info => { result = 1;  say(info.ExceptionMessage); };
				runner.OnTestOutput			= info => say(info.Output);
				runner.OnTestPassed 		= info => say(info.Output);
				runner.OnTestFinished 		= _ => say(".");
				runner.OnTestSkipped 		= _ => say("*");

				say("Discovering tests in assembly <" +  TestAssembly + ">");

				runner.Start(typeName, parallel: false);

				finished.WaitOne();
				finished.Dispose();

				say ("Tests finished with status <" + result + ">");

				return result == 0;
			}
		}
	}
}
