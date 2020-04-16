using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gentlemen.Domain
{
    public class Barber
    {
        public int BarberId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string ImagePath { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}