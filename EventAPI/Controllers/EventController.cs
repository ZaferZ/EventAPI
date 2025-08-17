
using Azure;
using EventAPI.Helpers;
using EventAPI.Models;
using EventAPI.Models.DTOs;
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
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            var response = await _eventService.GetAll();

            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEventsByUserId(Guid id)
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
        public async Task<ActionResult<IEnumerable<EventDto>>> GetLoggerEvents()
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
        public async Task<ActionResult<EventDto>> GetEventById(int id)
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

        [Authorize]
        [HttpPatch("add/{id}/{participantId}")]
        public async Task<ActionResult<UpdateEventDto>> AddParticipcant(int id, Guid participantId)
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
            catch (Exception e)
            {
                return NotFound(e.Message); 
            }
        }

        [Authorize]
        [HttpPatch("remove/{id}/{participantId}")]
        public async Task<ActionResult<EventDto>> RemoveParticipcant(int id, Guid participantId)
        {
            var userId = _jwtContext.UserId;
            if (participantId == Guid.Empty)
            {
                return BadRequest("Participant ID is invalid.");
            }
            try
            {
                var eventEntity = await _eventService.RemoveParticipant(id, participantId);
                if (eventEntity == null)
                {
                    return NotFound($"Event with ID {id} not found.");
                }
                return Ok(eventEntity);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto newEvent)
        {
            var userId = _jwtContext.UserId;
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (newEvent == null)
            {
                return BadRequest("Event data is null.");
            }
            var createdEvent = await _eventService.Create(newEvent, userId);


            return createdEvent;
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<UpdateEventDto>> UpdateEvent(int id, [FromBody] UpdateEventDto updatedEvent)
        {
            var userId = _jwtContext.UserId;
            try
            {
                var result = await _eventService.Update(updatedEvent, userId, id);
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
            
                await _eventService.Delete(id);
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
