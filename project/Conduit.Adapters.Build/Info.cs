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

			var project = new Project(BuildEngine.ProjectFileOfTaskNode);

			Cli.Say(BuildEngine, "PROPERTIES:{0}{0}", Environment.NewLine);

			foreach (ProjectProperty property in project.Properties)
			{
				Cli.Say(BuildEngine, "{0}: {1}", property.Name, property.EvaluatedValue);
			}

			Cli.Say(BuildEngine, "GLOBAL PROPERTIES: {0}{0}", Environment.NewLine);

			foreach (System.Collections.Generic.KeyValuePair<string,string> property in project.GlobalProperties)
			{
				Cli.Say(BuildEngine, "(GLOBAl) {0}: {1}", property.Key, property.Value);
			}

			return true;
		}
	}
}
