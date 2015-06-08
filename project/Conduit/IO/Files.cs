using System.IO;
using System.Linq;

namespace Conduit.IO
{
	public class Files
	{
		public static FileInfo[] Like(string txt)
		{
			return Directory.GetFiles(Directory.GetCurrentDirectory(), txt).Select(it => 
				new FileInfo(Path.GetFullPath(it))).ToArray();
		}
	}
}