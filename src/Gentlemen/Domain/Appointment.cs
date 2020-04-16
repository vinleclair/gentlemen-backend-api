using System;

namespace Gentlemen.Domain
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public DateTime ScheduledDate { get; set; }

        public int BarberId { get; set; }
        public Barber Barber { get; set; }
        
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}