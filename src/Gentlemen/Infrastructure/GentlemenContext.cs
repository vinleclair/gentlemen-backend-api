using Gentlemen.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Infrastructure
{
    public class GentlemenContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=gentlemen.db");
        
    }
}