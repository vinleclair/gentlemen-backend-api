using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gentlemen.Features.Appointments
{
    [Route("appointments")]
    public class ArticlesController : Controller
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<AppointmentEnvelope> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}