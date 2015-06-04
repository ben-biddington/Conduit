using Conduit.Integration.Tests.Versioning.Private;
using Conduit.UseCases.Semver.Semver;

namespace Conduit.Integration.Tests.Versioning
{
	internal static class AssemblyFileVersion
	{
		public static SemVersion For(string filename)
		{
			return AssemblyInfoVersion.For(filename, "AssemblyFileVersion");
		}
	}
}