using System;
using Newtonsoft.Json;

namespace Gentlemen.Domain
{
    public class Appointment
    {
        [JsonIgnore] public int AppointmentId { get; set; }

        public string ClientName { get; set; }

        public string ClientEmail { get; set; }
        
        public int BarberId { get; set; }
        
        public DateTime ScheduledDate { get; set; }
        
        public int ServiceId { get; set; }
    }
}