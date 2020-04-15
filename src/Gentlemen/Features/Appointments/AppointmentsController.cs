using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gentlemen.Features.Appointments
{
    [Route("appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<AppointmentEnvelope> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }
        
        [HttpGet("upcoming/{BarberId}")]
        public async Task<UpcomingAppointmentsEnvelope> getUpcomingAppointments(int BarberId)
        {
            return await _mediator.Send(new UpcomingAppointments.Query(BarberId));
        }
    }
}