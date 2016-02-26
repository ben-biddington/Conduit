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
            Assert.False(Conduit.Adapters.Build.Xunit.Run(TestReport.Silent, Path.Combine("res", "Example.Unit.Tests.dll")));
        }
    }
}

