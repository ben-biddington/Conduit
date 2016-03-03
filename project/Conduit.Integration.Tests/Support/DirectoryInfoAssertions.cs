using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Conduit.Integration.Tests.Support
{
    internal static class DirectoryInfoAssertions
    {
        internal static void MustContain(this DirectoryInfo self, params string[] expectedFiles)
        {
            var files = self.GetFiles();

            var actualNames = files.Select(it => it.Name).ToList();

            Assert.True(files.Length == expectedFiles.Length,
                $"Expected the dir <{self.FullName}> to contain <{expectedFiles.Length}> files, but it contains <{files.Length}>:{Environment.NewLine}{string.Join(Environment.NewLine, actualNames)}");

            Assert.True(actualNames.Intersect(expectedFiles).Count().Equals(expectedFiles.Length), 
                $@"Expected these files:
                
                  {string.Join(Environment.NewLine, expectedFiles)}
                    
                  Got:
                  
                {string.Join(Environment.NewLine, actualNames)}");
        }
    }
}