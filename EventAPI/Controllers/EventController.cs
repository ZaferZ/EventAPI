
using Azure;
using EventAPI.Models;
using EventAPI.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<EventGetDTO>>> GetAllEvents()
        {
            var response = await _eventService.GetAll();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventGetDTO>> GetEventById(int id)
        {
            try
            {
                var response = await _eventService.GetById(id);
                return Ok(response);
            }
            catch (KeyNotFoundException)     
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventGetDTO>> CreateEvent([FromBody] EventCreateDTO newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event data is null.");
            }
            var createdEvent = await _eventService.Create(newEvent);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Event>> UpdateEvent(int id, [FromBody] EventUpdateDTO updatedEvent)
        {
            if (updatedEvent == null || updatedEvent.Id != id)
            {
                return BadRequest("Event data is invalid.");
            }
            try
            {
                var result = await _eventService.Update(updatedEvent);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                Event eventToDelete = await _eventService.GetById(id);
                await _eventService.Delete(eventToDelete);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }
    }
}
