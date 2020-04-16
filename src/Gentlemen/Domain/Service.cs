using System.Collections.Generic;

namespace Gentlemen.Domain
{
    public class Service
    {
        public int ServiceId { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }
        public int Duration { get; set; }
        
        public List<Appointment> Appointments { get; set; }
    }
}