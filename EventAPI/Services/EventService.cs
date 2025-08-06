using EventAPI.Models;
using EventAPI.Repositories;
using Mapster;

namespace EventAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
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
            var events = await _eventRepository.GetAll();
            var response = events.Adapt<List<EventGetDTO>>();
            return await _eventRepository.GetById(id);
        }

        public async Task<Event> Create(EventCreateDTO newEvent)
        {

            TypeAdapterConfig<EventCreateDTO, Event>.NewConfig()
                .Map(d => d.Id, s => 0)
                .Map(d => d.CreatedAt, s => DateTime.UtcNow)
                .Map(d => d.CreatedBy, s => new Guid("7E61F925-B7D6-4E69-BBC2-A6695E2E424F")); // add the logged user id here

            var eventEntity = newEvent.Adapt<Event>();


            return await _eventRepository.Create(eventEntity);
        }

        public async Task<Event> Update(EventUpdateDTO newEvent)
        {
            TypeAdapterConfig<EventUpdateDTO, Event>.NewConfig()
                .Map(d => d.ModifiedAt, s => DateTime.UtcNow)
                .Map(d => d.ModifiedBy, s => new Guid("7E61F925-B7D6-4E69-BBC2-A6695E2E424F"))   // add the logged user id here
                .Map(d => d.CreatedBy, s => new Guid("7E61F925-B7D6-4E69-BBC2-A6695E2E424F"));  // add the logged user id here


            var eventEntity = newEvent.Adapt<Event>();

            return await _eventRepository.Update(eventEntity);
        }

        public async Task Delete(Event newEvent)
        {
            await _eventRepository.Delete(newEvent);
        }




    }
}
