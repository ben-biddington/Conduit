using System.IO;
using System.Linq;
using Conduit.Adapters.Build.Packaging;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging
{
    public class Can_read_a_packages_config_file : RunsInCleanRoom
    {
        [Fact]
        public void for_example() {
            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                        <packages>
                            <package id=""Microsoft.Web.Xdt"" version=""2.1.0"" targetFramework=""net45"" />
                            <package id=""Newtonsoft.Json"" version=""6.0.4"" targetFramework=""net45"" />
                            <package id=""xunit"" version=""2.1.0"" targetFramework=""net45"" />
                            <package id=""xunit.core"" version=""2.1.0"" targetFramework=""net45"" />
                        </packages>";

            var file = FileMachine.Make("packages.config", xml);

            var packages = PackagesConfig.Read(file);

            Assert.True(packages.Any(it => it.Id.Equals("xunit")), string.Join(",", packages.Select(it => it.Id)));
        }

        [Fact]
        public void it_returns_empty_when_file_does_not_exist() {
            var packages = PackagesConfig.Read(new FileInfo("xxx-does-not-exist-xxx"));

            Assert.Equal(0, packages.Count);
        }

        // TEST: Should really be able to supply text rather than file
        // TEST: Check that it returns all the right values
    }
}