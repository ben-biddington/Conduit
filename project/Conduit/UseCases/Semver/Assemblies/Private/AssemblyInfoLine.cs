using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Conduit.UseCases.Semver;
using Conduit.UseCases.Semver.Semver;
using System.Collections.Generic;

namespace Conduit.UseCases.Semver.Assemblies.Private
{
	internal static class AssemblyInfoLine 
	{
		internal static bool IsComment(string line)
		{
			return (line ?? string.Empty).TrimStart().StartsWith ("//");
		}

		internal static bool IsBlank(string line)
		{
			return string.IsNullOrEmpty((line ?? string.Empty).Trim());
		}

		internal static bool IsInstruction(string line)
		{
			return false == IsBlank(line) && false == IsComment(line);
		}
	}
	
}