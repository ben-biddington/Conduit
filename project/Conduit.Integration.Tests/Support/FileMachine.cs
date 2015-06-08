using System.IO;

namespace Conduit.Integration.Tests.Support
{
	internal static class FileMachine
	{
		public static FileInfo Make(string filename, string content)
		{
			using (var s = File.OpenWrite(filename))
			using (var writer = new StreamWriter(s))
			{
				writer.Write(content);
				writer.Flush();
				writer.Close();
			}

			return new FileInfo(Path.GetFullPath(filename));
		}
	}
}