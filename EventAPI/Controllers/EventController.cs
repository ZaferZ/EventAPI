using EventAPI.Data;
using EventAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventDbContext _context;

        public EventController(EventDbContext context)
        {
            _context = context;
        }

        // GET: api/Event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Event/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // POST: api/Event
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event @event)
        {
            @event.Id = Guid.NewGuid();
            @event.CreatedAt = DateTime.UtcNow;
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = @event.Id }, @event);
        }

        // PUT: api/Event/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            // Update fields
            existingEvent.Title = @event.Title;
            existingEvent.Description = @event.Description;
            existingEvent.OwnerId = @event.OwnerId;
            existingEvent.HobbyId = @event.HobbyId;
            existingEvent.StartDate = @event.StartDate;
            existingEvent.EndDate = @event.EndDate;
            existingEvent.Location = @event.Location;
            existingEvent.Capacity = @event.Capacity;
            existingEvent.ModifiedAt = DateTime.UtcNow;
            existingEvent.ModifiedBy = @event.ModifiedBy;
            existingEvent.ParticipantIds = @event.ParticipantIds;
            existingEvent.Status = @event.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Event/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
