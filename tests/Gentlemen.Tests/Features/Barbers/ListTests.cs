using System.Threading.Tasks;
using Xunit;

namespace Gentlemen.Tests.Features.Barbers
{
    public class ListTests : TestFixture
    {
        [Fact]
        public async Task Expect_List_Two_Barbers()
        {
            var barbersEnvelope = await BarberHelpers.ListBarbers(this);

            Assert.Equal(2, barbersEnvelope.Barbers.Count);
        }
    }
}