using System;
using Microsoft.Build.Utilities;

namespace Conduit.Build.Targets
{
	public class Xunit : Task
	{
		public String TestAssembly { get; set;}

		/// <summary>
		/// xbuild path/to/name.csproj /property:TestAssembly=path/to/AssemblyName.dll /t:Test
		/// </summary>
		public override bool Execute()
		{
			Cli.Say(BuildEngine, "Running test assembly <{0}>", TestAssembly);

			Action<string> say = msg => Cli.Say(BuildEngine, msg);

			return Conduit.Adapters.Build.Xunit.Run (say, TestAssembly);
		}
	}
}
