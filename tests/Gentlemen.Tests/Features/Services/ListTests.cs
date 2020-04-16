using System.Threading.Tasks;
using Xunit;

namespace Gentlemen.Tests.Features.Services
{
    public class ListTests : TestFixture
    {
        [Fact]
        public async Task Expect_List_Single_Service()
        {
            var servicesEnvelope = await ServiceHelpers.ListServices(this);

            Assert.Single(servicesEnvelope.Services);
        }
    }
}