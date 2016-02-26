﻿using System;
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

            var actual = Dir.Newest(new Filename("example.dll"));

            Assert.Equal(expected.FullName, actual.FullName);
        }

        [Fact]
        public void can_supply_wildcard_glob_pattern()
        {
            FileMachine.Touch("src", "bin", "Debug", "example.dll");

            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            var expected = FileMachine.Touch("src", "bin", "Release", "example.dll");

            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            FileMachine.Touch("src", "obj", "Debug", "example.dll");

            var actual = Dir.Newest(new Glob(@".\*\bin\*\example.dll"));

            Assert.Equal(expected.FullName, actual.FullName);
        }

        // TEST: I think the separators must match

        [Fact]
        public void single_match_is_okay_too()
        {
            var expected = FileMachine.Touch("bin", "Debug", "example.dll");

            var actual = Dir.Newest(new Filename("example.dll"));

            Assert.Equal(expected.FullName, actual.FullName);
        }

        [Fact]
        public void no_matches_returns_null()
        {
            FileMachine.Touch("bin", "Debug", "example.dll");

            Assert.Null(Dir.Newest(new Filename("xxx-does-not-exist-xxx.dll")));
        }
    }

    //internal static class Space
    //{
    //    internal static T Apart<T>(TimeSpan time, params Func<T>[] blocks)
    //    {
    //        //var list = Array.C
    //    }
    //}
}

