using System.Linq;
using System;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using Microsoft.Build.Execution;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace Conduit.Build.Targets
{
    public class List : Task
    {
        public bool IncludeImported { get; set; }

        public override bool Execute()
        {
            using (var reader = XmlReader.Create(new StringReader(File.ReadAllText(BuildEngine.ProjectFileOfTaskNode))))
            {
                var project = new Project(reader);

                var targets = Filter(project);

                Cli.Say(BuildEngine, "Project file <{0}> has the following <{1}> targets", BuildEngine.ProjectFileOfTaskNode, targets.Count);

                foreach (ProjectTargetInstance target in targets)
                {
                    Cli.Say(BuildEngine, "* {0}{1}", target.Name, Environment.NewLine);
                }

                return true;
            }
        }

        private List<ProjectTargetInstance> Filter(Project project)
        {
            if (IncludeImported)
                return project.Targets.Values.ToList();

            return project.Targets.Values.Where(it => it.Location.File.Equals(string.Empty)).OrderBy(it => it.Name).ToList();
        }
    }
}
