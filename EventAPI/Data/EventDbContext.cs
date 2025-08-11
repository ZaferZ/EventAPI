using EventAPI.Models;
using Microsoft.EntityFrameworkCore;
using EventAPI.Models.Associations;

namespace EventAPI.Data
{
    public class EventDbContext(DbContextOptions<EventDbContext> options):DbContext(options)
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<EventParticipant> EventParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
               .HasMany(e => e.Participants)
               .WithMany(e => e.Events)
               .UsingEntity<EventParticipant>();
        }



    }
}
