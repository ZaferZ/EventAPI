
using Azure;
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

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventGetDTO>>> GetAllEvents()
        {
            var response = await _eventService.GetAll();

            return Ok(response);
        }

        [Authorize(Roles = "user")]
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
                {
                    return Unauthorized("User ID is not available.");
                }
                if (userId != null)
                {
                    var response = await _eventService.GetByUserId(userGuid);
                    return Ok(response);
                }
                return BadRequest("User ID is not valid.");

            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Events for user with ID {userId} not found.");
            }
        }

        [Authorize(Roles = "user")]
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

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<ActionResult<EventGetDTO>> CreateEvent([FromBody] EventCreateDTO newEvent)
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            //{
            //    return Unauthorized("User ID is not available.");
            //}


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
