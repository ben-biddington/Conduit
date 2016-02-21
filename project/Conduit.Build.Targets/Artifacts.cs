using System.IO;
using Microsoft.Build.Utilities;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Evaluation;

namespace Conduit.Build.Targets
{
	public class Artifacts : Task
	{
		[Required]
		public string OutputDirectory { get; set; }
		public string SourceDirectory { get; set; }
		public string ArtifactName 		{ get; set; }

		public override bool Execute()
		{
			var project = new Project(BuildEngine.ProjectFileOfTaskNode);

			OutputDirectory = Path.GetFullPath(OutputDirectory);
			SourceDirectory = SourceDirectory ?? project.DirectoryPath;
			ArtifactName = ArtifactName ?? Path.GetFileNameWithoutExtension(project.FullPath);

			if (Directory.Exists (OutputDirectory)) {
				Directory.Delete (OutputDirectory, true);
			}
			
			if (File.Exists(OutputDirectory)) {
				File.Delete (OutputDirectory);
			}

			Directory.CreateDirectory (OutputDirectory);

			var to		= Path.Combine(Path.GetFullPath(OutputDirectory), string.Format("{0}.zip", ArtifactName));
			var from	= Path.GetFullPath(SourceDirectory);

			Cli.Say(BuildEngine, "Project file <{0}>", project.FullPath);

			// @todo: what we're after is `BaseOutputPath`, see <https://msdn.microsoft.com/en-us/library/bb629394.aspx>
			// We would like to be able to automatically collect build output -- automatically including the build config (debug or relrease)
			Cli.Say(BuildEngine, "Builds to <{0}>", project.GetPropertyValue("BaseOutputPath"));
			Cli.Say(BuildEngine, "Creating archive at <{0}> with ({1}) files from <{2}>", to, Directory.GetFiles(from, "*.*", SearchOption.AllDirectories).Count(), from);

			UseCases.Archiving.Archive.At(to, new DirectoryInfo(SourceDirectory));

			return true;
		}
	}
}