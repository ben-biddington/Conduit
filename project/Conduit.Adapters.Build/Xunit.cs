using System.Linq;
using System;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using System.Collections;

namespace Conduit.Adapters.Build
{
	public class Xunit : Task
	{
		public override bool Execute()
		{
			// Is there an Xunit sdk for running tests programmatically?
			// See: https://github.com/xunit/samples.xunit/blob/master/TestRunner/Program.cs
			Cli.Say(BuildEngine, "This is where we would run whatever tests you suply as arguments.");

			return true;
		}
	}
}
