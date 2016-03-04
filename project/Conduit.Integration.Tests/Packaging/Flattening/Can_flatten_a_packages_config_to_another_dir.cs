using System.IO;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging.Flattening
{
    public class Can_flatten_a_packages_config_to_another_dir : AcceptanceTest
    {
        [Fact]
        public void for_example()
        {
            Given_this_is_present_and_installed(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <packages>
                    <package id=""Conduit.Build.Targets""   version=""0.0.8"" targetFramework=""net45"" />
                    <package id=""xunit.runner.msbuild""    version=""2.1.0"" targetFramework=""net45"" />
                    <package id=""MSBuild.Extension.Pack""  version=""1.8.0"" targetFramework=""net45"" />
                </packages>");

            var targetDirectory = new DirectoryInfo("bin");

            When_I_flatten_to(targetDirectory);

            targetDirectory.MustContain(
               "Conduit.Adapters.Build.dll",
               "Conduit.Build.Targets.dll",
               "Conduit.dll",
               "HTML.xslt",
               "NUnitXml.xslt",
               "xunit.abstractions.dll",
               "xunit.runner.msbuild.dll",
               "xunit.runner.msbuild.props",
               "xunit.runner.reporters.desktop.dll",
               "xunit.runner.utility.desktop.dll",
               "xUnit1.xslt", 
               "_",
               "Interop.COMAdmin.dll",
               "Interop.IWshRuntimeLibrary.dll",
               "Ionic.Zip.dll",
               "MSBuild.Extension.Pack.targets",
               "MSBuild.ExtensionPack.BizTalk.dll",
               "MSBuild.ExtensionPack.Crm.dll",
               "MSBuild.ExtensionPack.Git.dll",
               "MSBuild.ExtensionPack.Iis7.dll",
               "MSBuild.ExtensionPack.Loggers.dll",
               "MSBuild.ExtensionPack.Sql2005.dll",
               "MSBuild.ExtensionPack.Sql2008.dll",
               "MSBuild.ExtensionPack.Sql2012.dll",
               "MSBuild.ExtensionPack.Sql2014.dll",
               "MSBuild.ExtensionPack.StyleCop.dll",
               "MSBuild.ExtensionPack.Sync.dll",
               "MSBuild.ExtensionPack.TaskFactory.Dlr.dll",
               "MSBuild.ExtensionPack.TaskFactory.PowerShell.dll",
               "MSBuild.ExtensionPack.Tfs.dll",
               "MSBuild.ExtensionPack.Tfs2010.dll",
               "MSBuild.ExtensionPack.Tfs2012.dll",
               "MSBuild.ExtensionPack.Tfs2013.dll",
               "MSBuild.ExtensionPack.VersionNumber.targets",
               "MSBuild.ExtensionPack.WindowsVirtualPC.dll",
               "MSBuild.ExtensionPack.dll",
               "MSBuild.ExtensionPack.dll.config",
               "MSBuild.ExtensionPack.pdb"
            );
        }

        // TEST: What do we do about the case where packages contain the same fiels under say lib/ and build/? That is the case in the test above

        // TEST: we are requiring the packages to have been installed already (?)
        // @todo: consider looking up the package (once of course) and having it know the files to expect
    }
}