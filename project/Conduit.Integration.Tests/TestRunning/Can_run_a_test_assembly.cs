using System.IO;
using Xunit;

namespace Conduit.Integration.Tests.TestRunning
{
    public class Can_run_a_test_assembly
    {
        [Fact]
        public void for_example()
        {
            Adapters.Build.Xunit.Run(_ => { }, Path.GetFullPath("TestRunning\\samples\\Conduit.Unit.Tests.dll"));
        }
    }
}