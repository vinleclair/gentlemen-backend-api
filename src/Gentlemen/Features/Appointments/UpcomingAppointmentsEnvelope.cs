using System;
using System.Collections.Generic;
using Gentlemen.Domain;

namespace Gentlemen.Features.Appointments
{
    public class UpcomingAppointmentsEnvelope
    {
        public UpcomingAppointmentsEnvelope(Dictionary<String, List<string>> upcomingAppointments)
        {
            UpcomingAppointments = upcomingAppointments;
        }

        public Dictionary<String, List<string>> UpcomingAppointments { get; set; }
    }
}