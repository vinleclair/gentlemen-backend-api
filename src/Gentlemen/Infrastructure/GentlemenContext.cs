﻿using Gentlemen.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Infrastructure
{
    public class GentlemenContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Service> Services { get; set; }

        public GentlemenContext()
        {
            // Parameterless constructor needed to run migrations
        }
        
        public GentlemenContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}