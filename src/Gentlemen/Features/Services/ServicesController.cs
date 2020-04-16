using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gentlemen.Features.Services
{
    [Route("services")]
    public class ServicesController : Controller
    {
        private readonly IMediator _mediator;

        public ServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ServicesEnvelope> Get()
        {
            return await _mediator.Send(new List.Query());
        }
    }
}