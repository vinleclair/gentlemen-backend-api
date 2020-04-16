using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Index("UX_Appointments_ScheduledDate_BarberId", IsUnique = true)]
        public DateTime ScheduledDate { get; set; }
        
        [Required]
        public int BarberId { get; set; }
        public Barber Barber { get; set; }
        
        [Required] 
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}