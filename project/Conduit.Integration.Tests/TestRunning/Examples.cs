using System;
using System.Reflection;
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
            var assembly = Resources.UnpackTo("Conduit.Integration.Tests.res.Example.Unit.Tests.dll", "Example.Unit.Tests.dll");

            Assert.True(Conduit.Adapters.Build.Xunit.Run(
                TestReport.Silent, 
                assembly.FullName, 
                null, 
                XunitOptions.NoAppDomain));
        }

        [Fact]
        public void this_one_shows_one_failure_and_one_skipped()
        {
            TestFailure actualFailed = null;

            var reporter = TestReport.Silent.WithFailure(it => actualFailed = it);

            var assembly = Resources.UnpackTo("Conduit.Integration.Tests.res.Example.Unit.Tests.Mixed.dll", $"{Guid.NewGuid()}.dll");

            Assert.False(Conduit.Adapters.Build.Xunit.Run(
                reporter, 
                assembly.FullName, 
                null, 
                XunitOptions.NoAppDomain), "Expected one of these tests to've failed, but they all passed");

            var expected = "Failing on purpose";

            Assert.True(actualFailed.Message.StartsWith(expected), $"Expected <{actualFailed.Message}> to start with <{expected}>");
        }

        // TEST: Make sure failures have the test name
    }

    internal static class Resources
    {
        internal static FileInfo UnpackTo(string name, string path)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(name))
            using (var writer = File.OpenWrite(path))
            {
                writer.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(writer);
            }

            return new FileInfo(path);
        }
    }
}

