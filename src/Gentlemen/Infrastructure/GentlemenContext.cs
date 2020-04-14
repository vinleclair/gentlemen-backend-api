using Gentlemen.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Infrastructure
{
    public class GentlemenContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Service> Services { get; set; }


        public GentlemenContext(DbContextOptions options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Barber>().HasData(new Barber {BarberId = 1, Name = "Matthew", PortraitPath="../assets/images/matthew.png"});
            modelBuilder.Entity<Barber>().HasData(new Barber {BarberId = 2, Name = "Fredrick", PortraitPath="../assets/images/fredrick.png"});
            
            modelBuilder.Entity<Barber>().HasData(new Service {ServiceId = 1, Name = "Haircut", Price = 26, Duration = 30});
            modelBuilder.Entity<Barber>().HasData(new Service {ServiceId = 2, Name = "Shave", Price = 20, Duration = 30});


        }
    }
}