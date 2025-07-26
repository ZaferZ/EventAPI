using EventAPI.Data;
using EventAPI.Models;
using EventAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("GetAllEvents")]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }
        [HttpGet("GetEventById/{id}")]
        public async Task<ActionResult<Event>> GetEventById(Guid id)
        {
            try
            {
                var eventItem = await _eventService.GetByIdAsync(id);
                return Ok(eventItem);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }

        [HttpPost("CreateEvent")]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event data is null.");
            }
            var createdEvent = await _eventService.CreateAsync(newEvent);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpPut("UpdateEvent/{id}")]
        public async Task<ActionResult<Event>> UpdateEvent(Guid id, [FromBody] Event updatedEvent)
        {
            if (updatedEvent == null || updatedEvent.Id != id)
            {
                return BadRequest("Event data is invalid.");
            }
            try
            {
                var result = await _eventService.UpdateAsync(updatedEvent);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }
        [HttpDelete("DeleteEvent/{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            try
            {
                Event eventToDelete = await _eventService.GetByIdAsync(id);
                await _eventService.DeleteAsync(eventToDelete);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }   
    }
}
