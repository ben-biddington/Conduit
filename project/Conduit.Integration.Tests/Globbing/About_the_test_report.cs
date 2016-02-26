using Conduit.Adapters.Build;
using Xunit;

namespace Conduit.Integration.Tests.Globbing
{
    public class About_the_test_report
    {
        [Fact]
        public void Quiet_has_all_no_ops()
        {
            var report = TestReport.Silent;

            Assert.NotNull(report.Log);
            Assert.NotNull(report.Finished);
            Assert.NotNull(report.Failed);
        }         
    }
}