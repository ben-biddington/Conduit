using System.Linq;
using System;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using System.Collections;

namespace Conduit.Adapters.Build
{

	public class Info : Task
	{
		public override bool Execute()
		{

			Cli.Say(BuildEngine, "Project file <{0}>", BuildEngine.ProjectFileOfTaskNode);

			foreach (DictionaryEntry variable in Environment.GetEnvironmentVariables())
			{
				Cli.Say(BuildEngine, "{0}: {1}", variable.Key, variable.Value);
			}

			return true;
		}
	}
}
