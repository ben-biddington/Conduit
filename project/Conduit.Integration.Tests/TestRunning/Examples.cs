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
            Assert.False(Conduit.Adapters.Build.Xunit.Run(
                TestReport.Silent, 
                Path.Combine("res", "Example.Unit.Tests.Mixed.dll"), 
                null, 
                XunitOptions.NoAppDomain));
        }
    }
}

