using Gentlemen.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Infrastructure
{
    public class GentlemenContext : DbContext
    {
        public GentlemenContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Barber> Barbers { get; set; }
    }
}