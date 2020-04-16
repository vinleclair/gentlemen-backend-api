using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gentlemen.Domain
{
    public class Service
    {
        public int ServiceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Duration { get; set; }
        
        public List<Appointment> Appointments { get; set; }
    }
}