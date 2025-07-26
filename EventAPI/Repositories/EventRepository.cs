using EventAPI.Data;
using EventAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _context;
        public EventRepository(EventDbContext context) 
        {
           _context = context;
        }

        public async Task<Event> CreateAsync(Event newEvent)
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events.ToListAsync();
        }


        public async Task<Event> GetByIdAsync(Guid id)  
        {
            return await _context.Events.FindAsync(id) 
                   ?? throw new KeyNotFoundException($"Event with ID {id} not found.");
        }

        public async Task<Event> UpdateAsync(Event newEvent)
        {
            _context.Events.Update(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task DeleteAsync(Event newEvent)
        {
            _context.Events.Remove(newEvent);
            await _context.SaveChangesAsync();
        }
    }
}
