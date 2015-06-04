using System.IO;

namespace Conduit.Integration.Tests.Versioning
{
	internal static class FileMachine
	{
		public static void Make(string filename, string content)
		{
			using (var s = File.OpenWrite(filename))
			using (var writer = new StreamWriter(s))
			{
				writer.Write(content);
			}
		}
	}
}