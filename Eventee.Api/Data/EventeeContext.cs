using Eventee.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventee.Api.Data
{
    public class EventeeContext : DbContext
    {
        public EventeeContext(DbContextOptions<EventeeContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<GetTogether> GetTogethers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GetTogether>()
                .HasMany(d => d.Subscribers)
                .WithMany(d => d.SubscribedGetTogethers);

            modelBuilder.Entity<User>()
                .HasMany(d => d.HostedGetTogethers)
                .WithOne(d => d.Hoster)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
