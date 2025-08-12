using EventAPI.Helpers;
using EventAPI.Models;
using EventAPI.Models.DTOs;
using EventAPI.Repositories;
using Mapster;

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
        public async Task<IEnumerable<Event>> GetAll()
        { 
            var events = await _eventRepository.GetAll();
            var response = events.Adapt<List<EventDto>>();
            return events;
        }

        public async Task<IEnumerable<EventDto>> GetByUserId(Guid userId)
        {
            var events = await _eventRepository.GetByUserId(userId);
            var response = events.Adapt<List<EventDto>>();
            return response;
        }

        public async Task<Event> GetById(int id)
        {
            var events = await _eventRepository.GetById(id);
            var response = events.Adapt<EventDto>();
            return events;
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

            var eventEntity = await _eventRepository.GetEventWithParticipants(eventId);
            if (eventEntity == null)
                throw new Exception("Event not found.");

            var user = await _eventRepository.GetUserById(userId);

            if (user == null)
                throw new Exception("User not found.");

            if (eventEntity.Participants.Any(p => p.Id == userId))
                throw new Exception("User is already a participant.");

            eventEntity.Participants.Add(user);
            var updatedEvent=  await _eventRepository.Update(eventEntity);
 
           // var response = updatedEvent.Adapt<EventDto>(); 
           var response = MapEventToDto(updatedEvent); 
            return response;

        }

        public async Task<EventDto> RemoveParticipant(int eventId, Guid userId)
        {
           var eventEntity = await _eventRepository.GetEventWithParticipants(eventId);
            if (eventEntity == null)
                throw new Exception("Event not found.");

            var user = await _eventRepository.GetUserById(userId);

            if (user == null)
                throw new Exception("User not found.");

            if (!eventEntity.Participants.Any(p => p.Id == userId))
                throw new Exception("User is not a participant.");
            user = eventEntity.Participants.FirstOrDefault(p => p.Id == userId);
            if (!eventEntity.Participants.Remove(user))
            {
                throw new Exception("User not found.");
            }
          
            var updatedEvent = await _eventRepository.Update(eventEntity);

            var response = MapEventToDto(updatedEvent);
            return response;

        }
        public async Task<EventDto> Update(UpdateEventDto newEvent, Guid userId)
        {
            TypeAdapterConfig<UpdateEventDto, Event>.NewConfig()
                 .Map(d => d.ModifiedAt, s => DateTime.UtcNow)
                 .Map(d => d.ModifiedBy, s => userId)   // add the logged user id here
                 .Map(d => d.CreatedBy, s => userId);  // add the logged user id here


            var eventEntity = newEvent.Adapt<Event>();
            var updatedEvent = await _eventRepository.Update(eventEntity);
            var response = updatedEvent.Adapt<EventDto>();

            return response;
        }
        public async Task Delete(Event newEvent)
        {
            await _eventRepository.Delete(newEvent);
        }



        private UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        private EventDto MapEventToDto(Event ev)
        {
            return new EventDto
            {
                Title = ev.Title,
                Description = ev.Description,
                StartDate = ev.StartDate,
                EndDate = ev.EndDate,
                Location = ev.Location,
                Capacity = ev.Capacity,
                Participants = ev.Participants?.Select(MapUserToDto).ToList() ?? new List<UserDto>()
            };
        }

    }
}
