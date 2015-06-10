using System.IO;
using Microsoft.Build.Utilities;
using System.Linq;
using Microsoft.Build.Framework;

namespace Conduit.Adapters.Build
{
	public class Artifacts : Task
	{
		public string OutputDirectory { get; set; }
		public string SourceDirectory { get; set; }

		[Required]
		public string ArtifactName 		{ get; set; }

		public override bool Execute()
		{
			OutputDirectory = Path.GetFullPath(OutputDirectory);

			if (Directory.Exists (OutputDirectory)) {
				Directory.Delete (OutputDirectory, true);
			}

			
			if (File.Exists(OutputDirectory)) {
				File.Delete (OutputDirectory);
			}

			Directory.CreateDirectory (OutputDirectory);

			var to		= Path.Combine(Path.GetFullPath(OutputDirectory), string.Format("{0}.zip", ArtifactName ));
			var from	= Path.GetFullPath(SourceDirectory);
			Cli.Say(BuildEngine, ArtifactName);
			Cli.Say(BuildEngine, "Creating archive at <{0}> with ({1}) files from <{2}>", to, Directory.GetFiles(from).Count(), from);

			UseCases.Archiving.Archive.At(to, new DirectoryInfo(SourceDirectory));

			return true;
		}
	}
}