using Conduit.Integration.Tests.Versioning.Private;
using Conduit.UseCases.Semver.Semver;

namespace Conduit.Integration.Tests.Versioning
{
	internal static class AssemblyVersion
	{
		public static SemVersion For(string filename)
		{
			return AssemblyInfoVersion.For(filename, "AssemblyVersion");
		}

		public static void BumpMajor(string filename)
		{
			throw new System.NotImplementedException();
		}
	}
}