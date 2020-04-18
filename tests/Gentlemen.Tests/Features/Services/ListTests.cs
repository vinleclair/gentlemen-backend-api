using System.Threading.Tasks;
using Xunit;

namespace Gentlemen.Tests.Features.Services
{
    public class ListTests : TestFixture
    {
        [Fact]
        public async Task Expect_List_Two_Services()
        {
            var servicesEnvelope = await ServiceHelpers.ListServices(this);

            Assert.Equal(2, servicesEnvelope.Services.Count);
        }
    }
}