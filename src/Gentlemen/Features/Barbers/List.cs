using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentlemen.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Features.Barbers
{
    public class List
    {
        public class Query : IRequest<BarbersEnvelope>
        {}

        public class QueryHandler : IRequestHandler<Query, BarbersEnvelope>
        {
            private readonly GentlemenContext _context;

            public QueryHandler(GentlemenContext context)
            {
                _context = context;
            }

            public async Task<BarbersEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var barbers = await _context.Barbers.OrderBy(x => x.Name).ToListAsync(cancellationToken);

                return new BarbersEnvelope()
                {
                    Barbers = barbers.ToList()
                };
            }
        }
    }
}