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
        
        public async Task<IEnumerable<Event>> GetAll()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByUserId(Guid userId)
        {
            var events = await _context.Events
                .Where(e => e.CreatedBy == userId || e.OwnerId == userId)
                .ToListAsync();
            return events;
        }

        public async Task<Event> GetById(int id)
        {
            return await _context.Events.FindAsync(id)
                   ?? throw new KeyNotFoundException($"Event with ID {id} not found.");
        }

        public async Task<Event> Create(Event newEvent)
        {

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<Event> Update(Event newEvent)
        {
            _context.Events.Update(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task Delete(Event newEvent)
        {
            _context.Events.Remove(newEvent);
            await _context.SaveChangesAsync();
        }

      
    }
}
