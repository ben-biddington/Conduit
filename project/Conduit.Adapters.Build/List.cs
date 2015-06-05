﻿using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;

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
}