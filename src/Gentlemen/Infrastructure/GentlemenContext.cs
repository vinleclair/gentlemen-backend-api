using Gentlemen.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Infrastructure
{
    public class GentlemenContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        
        public GentlemenContext(DbContextOptions options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Barber>().HasData(new Barber {BarberId = 1, Name = "Matthew", PortraitPath="../assets/images/matthew.png"});
            modelBuilder.Entity<Barber>().HasData(new Barber {BarberId = 2, Name = "Fredrick", PortraitPath="../assets/images/fredrick.png"});
        }
    }
}