using System.IO;
using Microsoft.Build.Utilities;

namespace Conduit.Adapters.Build
{
	public class Artifacts : Task
	{
		public string OutputDirectory { get; set; }

		public override bool Execute()
		{
			var dir = OutputDirectory;

			Cli.Say(BuildEngine, "Creating archive at <{0}>", dir);

			UseCases.Archiving.Archive.At(dir);

			return true;
		}
	}
}