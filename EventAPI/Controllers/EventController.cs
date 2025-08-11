
using Azure;
using EventAPI.Helpers;
using EventAPI.Models;
using EventAPI.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IJwtContext _jwtContext;

        public EventController(IEventService eventService, IJwtContext jwtContext)
        {
            _jwtContext = jwtContext;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventGetDTO>>> GetAllEvents()
        {
            var response = await _eventService.GetAll();

            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<EventGetDTO>>> GetEventsByUserId(Guid id)
        {

            try
            {
                var response = await _eventService.GetByUserId(id);
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Events for user with ID {id} not found.");
            }
        }

        [Authorize(Roles = "user")]
        [HttpGet("myevents")]
        public async Task<ActionResult<IEnumerable<EventGetDTO>>> GetLoggerEvents()
        {
            var userId = _jwtContext.UserId;
            try
            {

            
                    var response = await _eventService.GetByUserId(userId);
                if (response != null && response.Any())
                    return Ok(response);
                
                return BadRequest("User ID is not valid.");

            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Events for user with ID {userId} not found.");
            }
        }

        [Authorize(Roles = "user, admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<EventGetDTO>> GetEventById(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            {
                return Unauthorized("User ID is not available.");
            }
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
        public async Task<ActionResult<EventGetDTO>> AddParticipcant(int id, Guid participantId)
        {
            var userId = _jwtContext.UserId;
            if (participantId == Guid.Empty)
            {
                return BadRequest("Participant ID is invalid.");
            }
            try
            {
                var eventEntity = await _eventService.AddParticipant(id, participantId);
                if (eventEntity == null)
                {
                    return NotFound($"Event with ID {id} not found.");
                }
                return Ok(eventEntity);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EventGetDTO>> CreateEvent([FromBody] EventCreateDTO newEvent)
        {
            var userId = _jwtContext.UserId;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (newEvent == null)
            {
                return BadRequest("Event data is null.");
            }
            var createdEvent = await _eventService.Create(newEvent, userId);


            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<Event>> UpdateEvent(int id, [FromBody] EventUpdateDTO updatedEvent)
        {
            var userId = _jwtContext.UserId;
            if (updatedEvent == null || updatedEvent.Id != id)
            {
                return BadRequest("Event data is invalid.");
            }
            try
            {
                var result = await _eventService.Update(updatedEvent, userId);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }

        [Authorize(Roles = "admin, user")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            
            try
            {
                Event eventToDelete = await _eventService.GetById(id);
                if (eventToDelete.CreatedBy != _jwtContext.UserId && !_jwtContext.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    return Forbid("You do not have permission to delete this event.");
                }
                await _eventService.Delete(eventToDelete);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Event with ID {id} not found.");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("auth-test")]
        public IActionResult AuthenticatedOnlyEndPoint()
        {

            return Ok("you are authenticated");

        }

        [Authorize(Roles = "admin")]
        [HttpGet("auth-admin")]
        public IActionResult AdminOnlyEndPoint()
        {

            return Ok("you are an admin");

        }
    }
}
