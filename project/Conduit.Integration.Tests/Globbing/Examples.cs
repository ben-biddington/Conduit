using System;
using System.Threading;
using Conduit.Adapters.Build;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Globbing
{
    public class Examples : RunsInCleanRoom
    {
        [Fact]
        public void find_the_newest_file_by_name()
        {
            FileMachine.Touch("bin", "Debug", "example.dll");

            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            var expected = FileMachine.Touch("bin", "Release", "example.dll");

            var actual = Dir.Newest("example.dll");

            Assert.Equal(expected.FullName, actual.FullName);
        }

        [Fact]
        public void single_match_is_okay_too()
        {
            var expected = FileMachine.Touch("bin", "Debug", "example.dll");

            var actual = Dir.Newest("example.dll");

            Assert.Equal(expected.FullName, actual.FullName);
        }

        [Fact]
        public void no_matches_returns_null()
        {
            FileMachine.Touch("bin", "Debug", "example.dll");

            Assert.Null(Dir.Newest("xxx-does-not-exist-xxx.dll"));
        }
    }
}

