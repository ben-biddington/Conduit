using System;
using System.IO;
using System.Linq;
using Conduit.Adapters.Build.Packaging;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging
{
    public class Can_read_a_packages_config_file_and_have_it_include_dependencies : AcceptanceTest
    {
        [Fact]
        public void for_example_have_it_read_everything_from_a_packages_dir()
        {
            Given_this_is_present_and_installed_with_dependencies(
              @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                    <package id=""Conduit.Build.Targets"" version=""0.0.8"" targetFramework=""net45"" />
                </packages>");

            var packages = PackagesDirectory.Read(new DirectoryInfo("packages"));

            var names = packages.Select(it => it.Id).ToList();

            Assert.True(names.Any(it => it.Equals("Conduit.Build.Targets")), $"Expected <{"Conduit.Build.Targets"}>, got: {string.Join(Environment.NewLine, names)}");
            Assert.True(names.Any(it => it.Equals("Minimatch")), $"Expected <{"Conduit.Build.Targets"}>, got: {string.Join(Environment.NewLine, names)}");
        }
    }
}