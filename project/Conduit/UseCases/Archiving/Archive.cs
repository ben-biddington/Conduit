using System;

namespace Conduit.UseCases.Archiving
{
	public class Archive
	{
		public bool Contains (string rEADfileMEmd)
		{
			return false;	
		}

		public static Archive At (string examplezip)
		{
			return new Archive();
		}
	}
}

