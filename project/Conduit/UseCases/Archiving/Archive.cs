using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using Conduit.Lang;

namespace Conduit.UseCases.Archiving
{
	public class Archive
	{
		private readonly FileInfo _filename;

		public Archive(string filename)
		{
			_filename = new FileInfo(Path.GetFullPath(filename));
			With(_ => { });
		}

		public FileInfo Filename
		{
			get { return _filename; }
		}

		public IEnumerable<string> Contents()
		{
			using (var zipFileStream = File.OpenRead(Filename.Name))
			{
				var archive = new ZipArchive(zipFileStream);

				return archive.Entries.Select(it => it.Name).ToArray();
			};
		}

		public bool Contains(string filename)
		{
			using (var s = File.OpenRead(Filename.FullName)) {
				var zip = new ZipArchive(s);
				return zip.GetEntry(filename) != null;
			}
		}

		public static Archive At (string filename, params FileInfo[] files)
		{
			return new Archive(filename).Tap(it =>
			{
				foreach (var file in files)
				{
					it.Add(file);
				}
			});
		}

		public static Archive At(string filename, DirectoryInfo dir)
		{
			return new Archive(filename).Tap(it =>
			{
				foreach (var file in dir.EnumerateFiles())
				{
					it.Add(file);
				}
			});
		}

		public void Add(FileInfo fi)
		{
			if (false == fi.Exists)
				throw new FileLoadException("Cannot add a file that does not exist <" + fi.FullName + ">");

			With(package =>
			{
				var part = package.CreatePart(PackUriHelper.CreatePartUri(new Uri(fi.Name, UriKind.Relative)), "text/plain");

				using (var fileStream = File.OpenRead(fi.FullName))
				{
					fileStream.CopyTo(part.GetStream());
				}
			});
		}

		private void With(Action<Package> block)
		{
			using (Package p = Package.Open(Filename.FullName))
			{
				block(p);
			}
		}

		public void Open(FileInfo filename, Action<Stream> block)
		{
			using (var zipFileStream = File.OpenRead(Filename.Name))
			using (var @in = ArchiveItem(zipFileStream, filename).Open())
			{
				block(@in);
			};
		}

		private ZipArchiveEntry ArchiveItem(FileStream stream, FileInfo filename)
		{
			return ArchiveFrom(stream).GetEntry(filename.Name).Tap(it =>
			{
				if (null == it)
					throw new MissingFileError("The archive <{0}> does not contain a file called <{1}>", Filename.FullName, filename.Name);});
		}

		private static ZipArchive ArchiveFrom(FileStream zipFileStream)
		{
			return new ZipArchive(zipFileStream);
		}
	}
}