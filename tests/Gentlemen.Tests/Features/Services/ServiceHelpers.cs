using System.Threading;
using System.Threading.Tasks;
using Gentlemen.Features.Services;

namespace Gentlemen.Tests.Features.Services
{
    public class ServiceHelpers
    {
        public static async Task<ServicesEnvelope> ListServices(TestFixture fixture)
        {
            var dbContext = fixture.GetDbContext();

            var listQueryHandler = new List.QueryHandler(dbContext);
            var servicesList = await listQueryHandler.Handle(new List.Query(),
                new CancellationToken());

            return servicesList;
        }
    }
}