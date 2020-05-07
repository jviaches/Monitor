using Microsoft.EntityFrameworkCore;
using Monitor.Infra.Entities;
using System.Reflection;

namespace Monitor.Infra
{
    public class AppDbContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourcesHistory> ResourcesHistory { get; set; }
        public DbSet<UserAction> UserAction { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
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