using System;
using System.Threading;
using System.IO.Compression;
using System.IO;

namespace Conduit.UseCases.Archiving
{
	public class Archive
	{
		private string _filename;

		public Archive(string filename)
		{
			_filename = filename;
			using(var _ = File.Create(_filename));
		}

		public bool Contains (string filename)
		{
			using (var s = File.OpenRead(_filename)) {
				var zip = new ZipArchive(s);
				return zip.GetEntry (filename) != null;
			}
		}

		public static Archive At (string filename)
		{
			return new Archive (filename);
		}
	}
}

