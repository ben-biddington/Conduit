using Conduit.Integration.Tests.Support;
using Conduit.IO;
using Conduit.UseCases.Archiving;
using Xunit;

namespace Conduit.Integration.Tests.Archiving
{
    public class Can_add_globs_of_files : RunsInCleanRoom
    {
        [Fact]
        public void for_example()
        {
            FileMachine.Make("A.txt", "A");
            FileMachine.Make("B.txt", "B");
            FileMachine.Make("C.txt", "C");
            FileMachine.Make("D.html", "D");

            var archive = Archive.At("Example.zip", Files.Like("*.txt"));

            archive.MustContain("A.txt");
            archive.MustContain("B.txt");
            archive.MustContain("C.txt");

            archive.MustNotContain("D.html");
        }
    }
}