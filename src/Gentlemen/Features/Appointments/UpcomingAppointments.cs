using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentlemen.Domain;
using Gentlemen.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Features.Appointments
{
    public class UpcomingAppointments
    {
        public class Query : IRequest<UpcomingAppointmentsEnvelope>
        {
            public Query(int barberId)
            {
                BarberId = barberId;
            }
            
            public int BarberId { get; }
        }

        public class QueryHandler : IRequestHandler<Query, UpcomingAppointmentsEnvelope>
        {
            private readonly GentlemenContext _context;

            public QueryHandler(GentlemenContext context)
            {
                _context = context;
            }

            public async Task<UpcomingAppointmentsEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var upcomingAppointments = await _context.Appointments
                    .Where(a => a.BarberId == message.BarberId)
                    .Where(a => a.ScheduledDate >= DateTime.Now)
                    .Select(p => p.ScheduledDate)
                    .ToListAsync(cancellationToken);

                var parsedUpcomingAppointments = parseUpcomingAppointments(upcomingAppointments);

                return new UpcomingAppointmentsEnvelope(parsedUpcomingAppointments);
            }

            private Dictionary<string, List<string>> parseUpcomingAppointments(List<DateTime> upcomingAppointments)
            {
                var parsedUpcomingAppointments = new Dictionary<string, List<string>>();

                upcomingAppointments.ForEach(delegate(DateTime upcomingAppointment)
                {
                    var date = upcomingAppointment.ToString("yyyy-MM-dd");
                    var time = upcomingAppointment.ToString("HH:mm");

                    if (parsedUpcomingAppointments.ContainsKey(date))
                    {
                        List<string> timeslots = parsedUpcomingAppointments[date];
                        if (timeslots.Contains(time) == false)
                        {
                            timeslots.Add(time);
                        }
                    }
                    else
                    {
                        parsedUpcomingAppointments.Add(date, new List<string> {time});
                    }
                });
                
                return parsedUpcomingAppointments; 
            } 
        } 
    } 
}
