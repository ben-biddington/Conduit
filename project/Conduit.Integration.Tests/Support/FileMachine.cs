using System;
using System.IO;

namespace Conduit.Integration.Tests.Support
{
	internal static class FileMachine
	{
		public static FileInfo Make(string filename, string content)
		{
			var info = new FileInfo(filename);

			EnsureDirectoryExists(info);

			using (var s = File.OpenWrite(filename))
			using (var writer = new StreamWriter(s))
			{
				writer.Write(content);
				writer.Flush();
				writer.Close();
			}

			return new FileInfo(Path.GetFullPath(filename));
		}

		private static void EnsureDirectoryExists(FileInfo info)
		{
			if (false == Directory.Exists(info.DirectoryName))
				Directory.CreateDirectory(Path.GetFullPath(info.DirectoryName));
		}
	}
}