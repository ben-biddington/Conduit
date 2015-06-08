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
			return With(package => {
				return package.GetParts().Select(it => it.Uri.ToString()).ToArray();
			});
		}

		public bool Contains(string filename)
		{
			return With(package => 
				package.PartExists(PartUri(filename)));
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
				foreach (var file in dir.EnumerateFiles("*.*", SearchOption.AllDirectories))
				{
					it.Add(file); // @todo: part uri needs to b relative to the source dir
				}
			});
		}

		public void Add(FileInfo fi)
		{
			if (false == fi.Exists)
				throw new FileLoadException("Cannot add a file that does not exist <" + fi.FullName + ">");

			With(package =>
			{
				var part = package.CreatePart(PartUri(fi), "text/plain");

				using (var fileStream = File.OpenRead(fi.FullName))
				{
					fileStream.CopyTo(part.GetStream());
				}
			});
		}

		private static Uri PartUri(FileInfo fi)
		{
			return PartUri(fi.Name);
		}

		private static Uri PartUri(string filename)
		{
			return PackUriHelper.CreatePartUri(new Uri(filename, UriKind.Relative));
		}

		private T With<T>(Func<Package, T> block)
		{
			using (Package p = Package.Open(Filename.FullName))
			{
				return block(p);
			}
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
			With(package => {
				var part = package.GetPart(PartUri(filename.Name));

				if (null == part)
					throw new MissingFileError("The archive <{0}> does not contain a file called <{1}>", Filename.FullName, filename.Name);

				using (var s = part.GetStream())
				{
					block(s);
				}
			});
		}
	}
}