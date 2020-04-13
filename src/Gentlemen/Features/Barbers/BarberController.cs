using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gentlemen.Features.Barbers
{
    [Route("barbers")]
    public class BarbersController : Controller
    {
        private readonly IMediator _mediator;

        public BarbersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<BarbersEnvelope> Get()
        {
            return await _mediator.Send(new List.Query());
        }
    }
}