using EventAPI.Exceptions;
using EventAPI.Helpers;
using EventAPI.Models;
using EventAPI.Models.DTOs;
using EventAPI.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EventAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IJwtContext _jwtContext;
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _jwtContext = new JwtContext(new HttpContextAccessor());
            _eventRepository = eventRepository;
        }
        public async Task<IEnumerable<EventDto>> GetAll()
        {
            var events = await _eventRepository.GetAll();
            var response = events.Adapt<List<EventDto>>();
            return response;
        }

        public async Task<IEnumerable<EventDto>> GetByUserId(Guid userId)
        {
            var events = await _eventRepository.GetByUserId(userId);
            var response = events.Adapt<List<EventDto>>();
            return response;
        }

        public async Task<EventDto> GetById(int id)
        {
            var events = await _eventRepository.GetById(id);
            var response = events.Adapt<EventDto>();
            return response;
        }
        public async Task<EventDto> Create(CreateEventDto newEvent, Guid userId)
        {
            TypeAdapterConfig<CreateEventDto, Event>.NewConfig()
                .Map(d => d.Id, s => 0)
                .Map(d => d.CreatedAt, s => DateTime.UtcNow)
                .Map(d => d.CreatedBy, s => userId); // add the logged user id here

            var eventEntity = newEvent.Adapt<Event>();


            var entity = await _eventRepository.Create(eventEntity);
            var response = entity.Adapt<EventDto>();

            return response;
        }

        public async Task<EventDto> AddParticipant(int eventId, Guid userId)
        {

            var eventEntity = await _eventRepository.GetById(eventId);
            if (eventEntity == null)
                throw new NotFoundException("Event not found.");

            var user = await _eventRepository.GetUserById(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            if (eventEntity.Participants.Any(p => p.Id == userId))
                throw new ConflictException("User is already a participant.");

            eventEntity.Participants.Add(user);
            var updatedEvent = await _eventRepository.Update(eventEntity);

            // var response = updatedEvent.Adapt<EventDto>(); 

            var response = updatedEvent.Adapt<EventDto>();
            return response;

        }

        public async Task<EventDto> RemoveParticipant(int eventId, Guid userId)
        {
            var eventEntity = await _eventRepository.GetById(eventId);
            if (eventEntity == null)
                throw new NotFoundException("Event not found.");

            var user = await _eventRepository.GetUserById(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            if (!eventEntity.Participants.Any(p => p.Id == userId))
                throw new ConflictException("User is not a participant.");
            user = eventEntity.Participants.FirstOrDefault(p => p.Id == userId);
            if (!eventEntity.Participants.Remove(user))
            {
                throw new NotFoundException("User not found.");
            }

            var updatedEvent = await _eventRepository.Update(eventEntity);

            var response = updatedEvent.Adapt<EventDto>();
            return response;

        }

        public async Task<EventDto> Update(UpdateEventDto newEvent, Guid userId, int eventId)
        {

            var existingEvent = await _eventRepository.GetById(eventId);

            if (existingEvent == null)
                throw new NotFoundException($"Event {eventId} was not found.");


            newEvent.Adapt(existingEvent);
            existingEvent.ModifiedAt = DateTime.UtcNow;
            existingEvent.ModifiedBy = userId;
            var updatedEvent = await _eventRepository.Update(existingEvent);
            var response = updatedEvent.Adapt<EventDto>();

            return response;
        }
        public async Task Delete(int eventId)
        {

            var ev = await _eventRepository.GetById(eventId);
            if (ev == null)
                throw new KeyNotFoundException("Event not found");

            // Access control: allow only owner or admin
            AccessControl(ev);
            
                await _eventRepository.Delete(ev);
            
        }

        /// <summary>
        /// Checks if the current user has permission to manage the specified event.
        /// </summary>
        /// <param name="ev">The event to check access for.</param>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the current user is neither the event owner nor an admin.
        /// </exception>
        private void AccessControl(Event ev) {
            if (ev.OwnerId != _jwtContext.UserId && !_jwtContext.Role.Equals("admin"))
                throw new UnauthorizedAccessException("You are not allowed to delete this event");
        }

    }
}
