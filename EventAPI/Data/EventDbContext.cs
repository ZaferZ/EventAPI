using EventAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Data
{
    public class EventDbContext(DbContextOptions<EventDbContext> options):DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
    

        

    }
}
