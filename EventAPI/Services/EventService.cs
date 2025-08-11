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
            var eventEntity = await _eventRepository.GetById(eventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }
            if (eventEntity.ParticipantIds == null)
            {
                eventEntity.ParticipantIds = new List<Guid>();
            }
            if (!eventEntity.ParticipantIds.Contains(userId))
            {
                eventEntity.ParticipantIds.Add(userId);
                return await _eventRepository.Update(eventEntity);
            }
            else
            {
                throw new InvalidOperationException("User is already a participant in this event.");
            }
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
