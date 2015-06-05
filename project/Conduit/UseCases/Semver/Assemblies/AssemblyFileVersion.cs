using Conduit.UseCases.Semver.Semver;

namespace Conduit.UseCases.Semver.Assemblies
{

	public static class AssemblyFileVersion
	{
		public static SemVersion For(string filename)
		{
			return Version.For.File(filename, "AssemblyFileVersion");
		}

		public static void BumpMajor(string filename)
		{
			Conduit.UseCases.Semver.Assemblies.Private.AssemblyInfoVersion.BumpMajor(filename, "AssemblyVersion");
		}
	}
}