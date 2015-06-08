using System;
using System.IO.Compression;
using System.IO;
using System.IO.Packaging;

namespace Conduit.UseCases.Archiving
{
	public class Archive
	{
		private readonly string _filename;

		public Archive(string filename)
		{
			_filename = Path.GetFullPath(filename);
		}

		public bool Contains(string filename)
		{
			using (var s = File.OpenRead(_filename)) {
				var zip = new ZipArchive(s);
				return zip.GetEntry(filename) != null;
			}
		}

		public static Archive At (string filename)
		{
			return new Archive(filename);
		}

		public void Add(FileInfo fi)
		{
			if (false == fi.Exists)
				throw new FileLoadException("Cannot add a file that does not exist <" + fi.FullName + ">");

			using (Package p = Package.Open(_filename))
			{
				var part = p.CreatePart(PackUriHelper.CreatePartUri(new Uri(fi.Name, UriKind.Relative)), "text/plain");

				using (var fileStream = File.OpenRead(fi.FullName))
				{
					fileStream.CopyTo(part.GetStream());
				}
			}
		}

		public void Open(FileInfo filename, Action<Stream> block)
		{
			using (var zipFileStream = File.OpenRead(_filename))
			{
				using (var @in = new ZipArchive(zipFileStream).GetEntry(filename.Name).Open())
				{
					block(@in);
				};
			}
		}
	}
}