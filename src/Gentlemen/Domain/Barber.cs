using System.Collections.Generic;

namespace Gentlemen.Domain
{
    public class Barber
    {
        public int BarberId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        
        public List<Appointment> Appointments { get; set; }
    }
}