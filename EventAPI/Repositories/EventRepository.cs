using EventAPI.Data;
using EventAPI.Models;
using EventAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            var result = await _context.Events
                .Include(e => e.Participants)
                .Include(e => e.Comments)
                .Include(e => e.Tasks)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Event>> GetByUserId(Guid userId)
        {
            var events = await _context.Events
                .Where(e => e.CreatedBy == userId || e.OwnerId == userId)
                .Include(e =>e.Participants)
                .Include(e => e.Comments)
                .Include(e => e.Tasks)
                .ToListAsync();
            return events;
        }

        public async Task<Event> GetById(int id)
        {
            var events = await _context.Events
               .Include(e => e.Participants)
               .Include(e => e.Comments)
                .Include(e => e.Tasks)
               .FirstOrDefaultAsync(e => e.Id == id);

            return events
                   ?? throw new KeyNotFoundException($"Event with ID {id} not found.");
        }

        public async Task<Event> Create(Event ev)
        {

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return ev;
        }

        public async Task<Event> Update(Event ev)
        {
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();
            return ev;
        }

        public async Task Delete(Event ev)
        {
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
        }


    }
}
