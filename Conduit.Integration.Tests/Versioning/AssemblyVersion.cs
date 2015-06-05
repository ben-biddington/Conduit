using Conduit.Integration.Tests.Versioning.Private;
using Conduit.UseCases.Semver.Semver;

namespace Conduit.Integration.Tests.Versioning
{
	internal static class AssemblyVersion
	{
		public static SemVersion For(string filename)
		{
			return Conduit.Integration.Tests.Versioning.AssemblyInfoVersion.For.File(filename, "AssemblyVersion");
		}

		public static void BumpMajor(string filename)
		{
			Conduit.Integration.Tests.Versioning.Private.AssemblyInfoVersion.BumpMajor(filename, "AssemblyVersion");
		}
	}
}