using System;
using System.IO;

namespace Conduit.Integration.Tests.Support
{
	internal static class FileMachine
	{
		public static void Touch(params string[] parts)
		{
			Make(Path.Combine(parts), string.Empty);
		}

		public static void Touch(string filename)
		{
			Make(filename, string.Empty);
		}

		public static FileInfo Make(string filename, string content)
		{
			var info = new FileInfo(Path.GetFullPath(filename).Replace("\\", Path.DirectorySeparatorChar.ToString()));

			EnsureDirectoryExists(info);

			Write(info, content);

			return info;
		}

		static void Write(FileInfo filename, string content)
		{
				using (var s = File.OpenWrite(filename.FullName))
				using (var writer = new StreamWriter (s)) {
					writer.Write (content);
					writer.Flush ();
					writer.Close ();
				}
		}

		private static void EnsureDirectoryExists(FileInfo info)
		{
			if (false == Directory.Exists(info.DirectoryName))
				Directory.CreateDirectory(Path.GetFullPath(info.DirectoryName));
		}
	}
}