using Gentlemen.Domain;

namespace Gentlemen.Features.Appointments
{
    public class AppointmentEnvelope
    {
        public AppointmentEnvelope(Appointment appointment)
        {
            Appointment = appointment;
        }

        public Appointment Appointment { get; }
    }
}