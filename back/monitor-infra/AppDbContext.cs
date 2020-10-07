using Microsoft.EntityFrameworkCore;
using monitor_infra.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace monitor_infra
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserAction> UserActions { get; set; }
        public DbSet<CommunicationChanel> CommunicationChanels { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<MonitorItem> MonitorItems { get; set; }
        public DbSet<MonitorHistory> MonitorHistory { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}