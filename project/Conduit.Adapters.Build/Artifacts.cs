using System.IO;
using Microsoft.Build.Utilities;

namespace Conduit.Adapters.Build
{
	public class Artifacts : Task
	{
		public string OutputDirectory { get; set; }
		public string SourceDirectory { get; set; }

		public override bool Execute()
		{
			var to		= Path.GetFullPath(OutputDirectory);
			var from	= Path.GetFullPath(SourceDirectory);

			Cli.Say(BuildEngine, "Creating archive at <{0}> with files from <{1}>", to, from);

			UseCases.Archiving.Archive.At(to, new DirectoryInfo(SourceDirectory));

			return true;
		}
	}
}