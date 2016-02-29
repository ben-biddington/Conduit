using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Semver.Semver;
using System.IO;
using Conduit.UseCases.Semver.Assemblies;
using Xunit;

namespace Conduit.Integration.Tests.Versioning
{
    public class Can_bump_an_assembly_version : RunsInCleanRoom
    {
        [Fact]
        public void for_example()
        {
            FileMachine.Make("AssemblyInfo.cs", @"
                // The following GUID is for the ID of the typelib if this project is exposed to COM
                [assembly: Guid(""278c86e3-1ac7-4a3f-a0c6-f1e072422836"")]

                // Version information for an assembly consists of the following four values:
                //
                //      Major Version
                //      Minor Version 
                //      Build Number
                //      Revision
                //
                // You can specify all the values or you can default the Build and Revision Numbers 
                // by using the '*' as shown below:
                // [assembly: AssemblyVersion(""1.0.*"")]
                [assembly: AssemblyVersion(""0.0.0.*"")]
                [assembly: AssemblyFileVersion(""0.0.0.*"")]");

            AssemblyVersion.BumpMajor("AssemblyInfo.cs");

            Assert.Equal(@"
                // The following GUID is for the ID of the typelib if this project is exposed to COM
                [assembly: Guid(""278c86e3-1ac7-4a3f-a0c6-f1e072422836"")]

                // Version information for an assembly consists of the following four values:
                //
                //      Major Version
                //      Minor Version 
                //      Build Number
                //      Revision
                //
                // You can specify all the values or you can default the Build and Revision Numbers 
                // by using the '*' as shown below:
                // [assembly: AssemblyVersion(""1.0.*"")]
                [assembly: AssemblyVersion(""1.0.0.*"")]
                [assembly: AssemblyFileVersion(""1.0.0.*"")]", 
                File.ReadAllText("AssemblyInfo.cs"));
        }

        [Fact]
        public void it_changes_assembly_file_version_too() {
            FileMachine.Make("AssemblyInfo.cs", @"
                [assembly: AssemblyVersion(""0.0.0.*"")]
                [assembly: AssemblyFileVersion(""0.0.0.*"")]");

            AssemblyVersion.BumpMajor("AssemblyInfo.cs");

            Assert.True(TextFile.Contains ("AssemblyInfo.cs", @"AssemblyVersion(""1.0.0.*"")"), 
                @"Expected this text to contain <""AssemblyVersion(""1.0.0.*""))"">: " + File.ReadAllText ("AssemblyInfo.cs"));

            Assert.True(TextFile.Contains ("AssemblyInfo.cs", @"AssemblyFileVersion(""1.0.0.*"")"), 
                @"Expected this text to contain <""AssemblyFileVersion(""1.0.0.*""))"">: " + File.ReadAllText ("AssemblyInfo.cs"));
        }

        [Fact]
        public void it_preserves_everything_under_patch()
        {
            FileMachine.Make ("AssemblyInfo.cs", @"
				[assembly: AssemblyVersion(""0.0.0.*"")]");

            AssemblyVersion.BumpMajor ("AssemblyInfo.cs");

            Assert.True (TextFile.Contains ("AssemblyInfo.cs", @"AssemblyVersion(""1.0.0.*"")"), 
                @"Expected this text to contain <""AssemblyVersion(""1.0.0.*""))"">: " + File.ReadAllText ("AssemblyInfo.cs"));
        }

        [Fact]
        public void it_ignores_missing_build()
        {
            FileMachine.Make ("AssemblyInfo.cs", @"
                [assembly: AssemblyVersion(""0.0.0"")]");

            AssemblyVersion.BumpMajor("AssemblyInfo.cs");

            Assert.True (TextFile.Contains("AssemblyInfo.cs", @"AssemblyVersion(""1.0.0"")"), 
                @"Expected this text to contain <""AssemblyVersion(""1.0.0""))"">: " + File.ReadAllText ("AssemblyInfo.cs"));
        }
    }
}