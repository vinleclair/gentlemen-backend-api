using System;
using System.ComponentModel.DataAnnotations;

namespace Gentlemen.Domain
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        [Required] 
        public string ClientName { get; set; }
        [Required]
        public string ClientEmail { get; set; }
        [Required]
        public DateTime ScheduledDate { get; set; }
        
        [Required]
        public int BarberId { get; set; }
        public Barber Barber { get; set; }
        
        [Required] 
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}