using Conduit.Integration.Tests.Support;
using Xunit;
using Glob = Conduit.Build.Targets.Glob;

namespace Conduit.Integration.Tests.Globbing.With_msbuild
{
    public class About_the_msbuild_target : RunsInCleanRoom
    {
        [Fact]
        public void pattern_may_be_an_array()
        {
            FileMachine.Touch("obj", "Debug", "example.dll");
            FileMachine.Touch("obj", "Release", "example.dll");

            var expected = FileMachine.Touch("example.dll");

            var target = new Glob { Pattern = new [] { @".\example.dll", @".\xxx\example.dll;" } };

            target.Execute();

            Assert.Equal(expected.FullName, target.Path);
        }
    }
}