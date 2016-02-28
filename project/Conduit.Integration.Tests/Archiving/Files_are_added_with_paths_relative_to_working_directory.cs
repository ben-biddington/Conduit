using System;
using System.IO;
using Conduit.Integration.Tests.Support;
using Conduit.UseCases.Archiving;
using Xunit;

namespace Conduit.Integration.Tests.Archiving
{
    public class Files_are_added_with_paths_relative_to_working_directory : RunsInCleanRoom
    {
        [Fact]
        public void for_example()
        {
            FileMachine.Make("A\\A.txt", "A");
            FileMachine.Make("A\\BB\\A.txt", "A");
            FileMachine.Make("A\\BB\\CC\\A.txt", "A");

            var pwd = Environment.CurrentDirectory;

            var archive = Archive.At("Example.zip", new DirectoryInfo("A"));

            foreach (var path in archive.Contents())
            {
                Assert.False(path.Contains(pwd), $"Expected <{path}> to NOT contain the working directory <{pwd}>");
            }
        }
    }
}