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

        public DbSet<User> Users { get; set; }
        public DbSet<GetTogether> GetTogethers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GetTogether>()
                .HasMany(d => d.Subscribers)
                .WithMany(d => d.SubscribedGetTogethers);

            modelBuilder.Entity<GetTogether>()
                .HasOne(d => d.Hoster)
                .WithMany(d => d.HostedGetTogethers);
        }
    }
}
