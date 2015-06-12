using System.Linq;
using System;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using System.Collections;

namespace Conduit.Adapters.Build
{
	public class List : Task
	{
		public override bool Execute()
		{
			var project = new Project(BuildEngine.ProjectFileOfTaskNode);

			Cli.Say(BuildEngine, "Project file <{0}> has the following <{1}> targets", BuildEngine.ProjectFileOfTaskNode, project.Targets.Count);

			foreach (var target in project.Targets.OrderBy(it => it.Key))
			{
				Cli.Say(BuildEngine, "- {0}", target.Key, target.GetType());
			}

			return true;
		}
	}

	public class Nunit : Task
	{
		public override bool Execute()
		{
			var project = new Project(BuildEngine.ProjectFileOfTaskNode);

			// @todo: actually we need a console runner foundation class.
			// ~/Downloads/NUnit-2.6.4/bin/nunit-console.exe project/Conduit.Integration.Tests/bin/Debug/Conduit.Integration.Tests.dll

			return true;
		}
	}
}
