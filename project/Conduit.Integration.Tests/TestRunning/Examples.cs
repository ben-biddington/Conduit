using System;
using System.IO;
using Conduit.Adapters.Build;
using Xunit;

namespace Conduit.Integration.Tests
{
    public class Examples
    {
        [Fact]
        public void this_one_passes()
        {
            Assert.True(Conduit.Adapters.Build.Xunit.Run(
                TestReport.Silent, 
                Path.Combine("res", "Example.Unit.Tests.dll"), 
                null, 
                XunitOptions.NoAppDomain));
        }

        [Fact]
        public void this_one_shows_one_failure_and_one_skipped()
        {
            TestFailure actualFailed = null;

            var reporter = TestReport.Silent.WithFailure(it => actualFailed = it);

            Assert.False(Conduit.Adapters.Build.Xunit.Run(
                reporter, 
                Path.Combine("res", "Example.Unit.Tests.Mixed.dll"), 
                null, 
                XunitOptions.NoAppDomain));

            var expected = "Failing on purpose";

            Assert.True(actualFailed.Message.StartsWith(expected), $"Expected <{actualFailed.Message}> to start with <{expected}>");
        }

        // TEST: Mae sure failures have the test name
    }
}

