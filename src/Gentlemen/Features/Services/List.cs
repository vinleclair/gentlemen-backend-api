using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentlemen.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Features.Services
{
    public class List
    {
        public class Query : IRequest<ServicesEnvelope>
        {
        }

        public class QueryHandler : IRequestHandler<Query, ServicesEnvelope>
        {
            private readonly GentlemenContext _context;

            public QueryHandler(GentlemenContext context)
            {
                _context = context;
            }

            public async Task<ServicesEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var services = await _context.Services.OrderBy(x => x.Name).ToListAsync(cancellationToken);

                return new ServicesEnvelope()
                {
                    Services = services.ToList()
                };
            }
        }
    }
}