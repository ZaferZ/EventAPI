using EventAPI.Data;
using EventAPI.Models;
using EventAPI.Models.DTOs;
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
            var result = await _context.Events
                .Include(e => e.Participants)
                .ToListAsync();
            return result;
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

        public async Task<Event> GetEventWithParticipants(int eventId)
        {
            var participants = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == eventId);
            return participants;
        }

        public async Task<User> GetUserById(Guid userId)
        {
           
            var user = await _context.Users
            .Where(u => u.Id == userId)
             .Select(u => new User
             {
                 Id = u.Id,
                 Username = u.Username,
                 FirstName = u.FirstName,
                 LastName = u.LastName,
                 Email = u.Email
             })
                .FirstOrDefaultAsync();
            return user;
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
