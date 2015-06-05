using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Semver.Semver;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System;
using Conduit.UseCases.Semver.Assemblies;

namespace Conduit.Integration.Tests.Support
{
	internal static class TextFile {
		internal static bool Contains(string filename, string expected) 
		{
			foreach (var line in System.IO.File.ReadAllLines(filename).Where(it => false == string.IsNullOrEmpty(it)))
			{
				if (line.Contains(expected))
					return true;
			}

			return false;
		}
	}
}