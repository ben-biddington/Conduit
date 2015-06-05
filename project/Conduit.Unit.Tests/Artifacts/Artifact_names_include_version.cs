using System;
using NUnit.Framework;
using Conduit.UseCases.Semver.Semver;

namespace Conduit.Unit.Tests
{
	public class Artifact_names_include_version
	{
		[Test]
		public void for_example() 
		{
			var version = new SemVersion(1);

			var name = new ArtifactName(version);

			Assert.That (name.ToString(), Is.StringContaining("1.0.0"));
		}
	}

	class ArtifactName 
	{
		private readonly SemVersion _version;

		internal ArtifactName(SemVersion version)
		{
			_version = version;
		}

		public override string ToString()
		{
			return _version.ToString();
		}
	}
}

