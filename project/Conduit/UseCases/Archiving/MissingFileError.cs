using System;

namespace Conduit.UseCases.Archiving
{
	public class MissingFileError : Exception
	{
		public MissingFileError(string message, params object[] args) : base(string.Format(message, args))
		{
			
		}	 
	}
}