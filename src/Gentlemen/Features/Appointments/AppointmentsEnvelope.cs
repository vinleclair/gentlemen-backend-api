using System.Collections.Generic;
using Gentlemen.Domain;

namespace Gentlemen.Features.Appointments
{
    public class AppointmentsEnvelope
    {
        public List<Appointment> Appointments { get; set; }

        public int AppointmentsCount { get; set; }
    }
}