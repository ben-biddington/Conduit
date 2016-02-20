using System.Linq;
using System;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

namespace Conduit.Adapters.Build
{
	public class List : Task
	{
		public override bool Execute()
		{
			using (var reader = XmlReader.Create(new StringReader(File.ReadAllText(BuildEngine.ProjectFileOfTaskNode))))
			{
				var project = new Project(reader);

				Cli.Say(BuildEngine, "Project file <{0}> has the following <{1}> targets", BuildEngine.ProjectFileOfTaskNode, project.Targets.Count);

				foreach (var target in project.Targets.OrderBy(it => it.Key))
				{
					Cli.Say(BuildEngine, "- {0}", target.Key, target.GetType());
				}

				return true;
			}
		}
	}
}
