using Gentlemen.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gentlemen.Infrastructure
{
    public class GentlemenContext : DbContext
    {
        public GentlemenContext(DbContextOptions<GentlemenContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Service> Services { get; set; }
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