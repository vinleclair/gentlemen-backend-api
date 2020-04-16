using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Gentlemen.Features.Appointments;
using Xunit;

namespace Gentlemen.Tests.Features.Barbers
{
    public class ListTests : TestFixture
    {
        [Fact]
        public async Task Expect_List_Single_Barber()
        {
            var barbersEnvelope = await BarberHelpers.ListBarbers(this);

            Assert.Single(barbersEnvelope.Barbers);
        }
    }
}