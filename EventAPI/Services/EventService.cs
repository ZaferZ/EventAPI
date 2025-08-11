using EventAPI.Helpers;
using EventAPI.Models;
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
        public async Task<IEnumerable<EventGetDTO>> GetAll()
        {
            var events = await _eventRepository.GetAll();
            var response = events.Adapt<List<EventGetDTO>>();
            return response;
        }

        public async Task<IEnumerable<EventGetDTO>> GetByUserId(Guid userId)
        {
            var events = await _eventRepository.GetByUserId(userId);
            var response = events.Adapt<List<EventGetDTO>>();
            return response;
        }

        public async Task<Event> GetById(int id)
        {
            var events = await _eventRepository.GetById(id);
            return events;
        }
        public async Task<Event> Create(EventCreateDTO newEvent, Guid userId)
        {
            TypeAdapterConfig<EventCreateDTO, Event>.NewConfig()
                .Map(d => d.Id, s => 0)
                .Map(d => d.CreatedAt, s => DateTime.UtcNow)
                .Map(d => d.CreatedBy, s => userId); // add the logged user id here

            var eventEntity = newEvent.Adapt<Event>();


            return await _eventRepository.Create(eventEntity);
        }

        public async Task<Event> AddParticipant(int eventId, Guid userId)
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
 

            return updatedEvent;

        }

        public async Task<Event> RemoveParticipant(int eventId, Guid userId)
        {
           var eventEntity = await _eventRepository.GetEventWithParticipants(eventId);
            if (eventEntity == null)
                throw new Exception("Event not found.");

            var user = await _eventRepository.GetUserById(userId);

            if (user == null)
                throw new Exception("User not found.");

            if (!eventEntity.Participants.Any(p => p.Id == userId))
                throw new Exception("User is not a participant.");

            eventEntity.Participants.Remove(user);
            return await _eventRepository.Update(eventEntity);

        }
        public async Task<Event> Update(EventUpdateDTO newEvent, Guid userId)
        {
            TypeAdapterConfig<EventUpdateDTO, Event>.NewConfig()
                 .Map(d => d.ModifiedAt, s => DateTime.UtcNow)
                 .Map(d => d.ModifiedBy, s => userId)   // add the logged user id here
                 .Map(d => d.CreatedBy, s => userId);  // add the logged user id here


            var eventEntity = newEvent.Adapt<Event>();

            return await _eventRepository.Update(eventEntity);
        }
        public async Task Delete(Event newEvent)
        {
            await _eventRepository.Delete(newEvent);
        }

       
    }
}
