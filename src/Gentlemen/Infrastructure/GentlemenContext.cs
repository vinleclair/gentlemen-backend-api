using Gentlemen.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gentlemen.Infrastructure
{
    public class GentlemenContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Customer> Customers { get; set; }
        
        public GentlemenContext(DbContextOptions<GentlemenContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region BarberSeed
            modelBuilder.Entity<Barber>().HasData(new Barber {BarberId = 1, ImagePath = "/img/matthew.png", Name = "Matthew"});
            modelBuilder.Entity<Barber>().HasData(new Barber {BarberId = 2, ImagePath = "/img/fredrick.png", Name = "Fredrick"});
            #endregion

            #region ServiceSeed
            modelBuilder.Entity<Service>().HasData(new Service() { ServiceId = 1, Duration = 30, Name = "Haircut", Price = 26, ImagePath = "/img/haircut.png"});
            modelBuilder.Entity<Service>().HasData(new Service() { ServiceId = 2, Duration = 30, Name = "Shave", Price = 20, ImagePath = "/img/shave.png"});
            #endregion
        }
    }

    public class GentlemenContextFactory : IDesignTimeDbContextFactory<GentlemenContext>
    {
        // Used only for EF .NET Core CLI tools (update database/migrations etc.)
        public GentlemenContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GentlemenContext>();
            optionsBuilder.UseSqlite("Data Source=gentlemen.db");

            return new GentlemenContext(optionsBuilder.Options);
        }
    }
}