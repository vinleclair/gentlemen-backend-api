using System.Threading;
using System.Threading.Tasks;
using Gentlemen.Features.Barbers;

namespace Gentlemen.Tests.Features.Barbers
{
    public class BarberHelpers
    {
        public static async Task<BarbersEnvelope> ListBarbers(TestFixture fixture)
        {
            var dbContext = fixture.GetDbContext();

            var listQueryHandler = new List.QueryHandler(dbContext);
            var barbersList = await listQueryHandler.Handle(new List.Query(),
                new CancellationToken());

            return barbersList;
        }
    }
}