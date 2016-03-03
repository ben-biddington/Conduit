using Conduit.Adapters.Build;
using Conduit.Adapters.Build.IO;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Globbing
{
    public class You_may_supply_a_list_of_globs : RunsInCleanRoom
    {
        [Fact]
        public void for_example_the_first_glob_may_not_match_at_all()
        {
            FileMachine.Touch("obj", "Debug", "example.dll");
            FileMachine.Touch("obj", "Release", "example.dll");

            var expected = FileMachine.Touch("example.dll");

            var actual = Dir.Newest(new Glob(@".\xxx\example.dll"), new Glob(@".\example.dll"));

            Assert.Equal(expected.FullName, actual.FullName);
        }

        [Fact]
        public void and_it_returns_the_most_recently_modified_across_all_matches()
        {
            FileMachine.Touch("bin", "Debug"    , "example.dll");
            FileMachine.Touch("bin", "Release"  , "example.dll");

            var expected = FileMachine.Touch("example.dll");

            var actual = Dir.Newest(new Glob(@".\bin\**\example.dll"), new Glob(@".\example.dll"));

            Assert.Equal(expected.FullName, actual.FullName);
        }
    }
}