using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using System.Collections.Generic;

namespace Conduit.UseCases.Semver.Assemblies.Private
{
	internal static class Matching
	{
		private const string VERSION_PATTERN = @"\(""(?<versionstring>([^.]+).([^.]+).([^.]+))(?<suffix>.+)";

		internal static string Pattern(string prefix) {
			return string.Format(
				@"(?<preamble>.+)(?<prefix>{0})\(""(?<versionstring>([^.]+).([^.]+).([^.]+))(?<suffix>.+)",
				prefix);
		}
	}
	
}