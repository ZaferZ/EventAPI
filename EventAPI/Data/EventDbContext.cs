using EventAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Data
{
    public class EventDbContext(DbContextOptions<EventDbContext> options):DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Event>().HasData(
        //        new Event
        //        {
        //            Id = new Guid("12345678-1234-1234-1234-1234567890ab"),
        //            Title = "Football game",
        //            Description = "This will be a football game in a small closed fiels 6 against 6.",
        //            OwnerId = new Guid("22345678-1234-1234-1234-1234567890ab"),
        //            HobbyId = new Guid("32345678-1234-1234-1234-1234567890ab"),
        //            StartDate = new DateTime(2025, 07, 26,22,00,00),
        //            EndDate = new DateTime(2025, 07, 26, 23, 00, 00),
        //            Location = "Auto KA Sliven",
        //            Capacity = 12,
        //            CreatedBy = new Guid("42345678-1234-1234-1234-1234567890ab"),
        //            Status = EventStatus.Scheduled
        //        }
        //   );
 
    }
}
