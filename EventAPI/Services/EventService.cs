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
        public async Task<Event> CreateAsync(EventCreateDTO newEvent)
        {
            TypeAdapterConfig<EventCreateDTO, Event>.NewConfig()
                .Map(d => d.Id, s => 0)
                .Map(d => d.CreatedAt, s => DateTime.UtcNow)
                .Map(d => d.CreatedBy, s => new Guid()); // add the logged user id here

            var eventEntity = newEvent.Adapt<Event>();

            return await _eventRepository.CreateAsync(eventEntity);
        }

        public async Task DeleteAsync(Event newEvent)
        {
            await _eventRepository.DeleteAsync(newEvent);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _eventRepository.GetAllAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }

        public async Task<Event> UpdateAsync(EventUpdateDTO newEvent)
        {
            TypeAdapterConfig<EventUpdateDTO, Event>.NewConfig()
               .Map(d => d.ModifiedAt, s => DateTime.UtcNow)
               .Map(d => d.ModifiedBy, s => new Guid());// add the logged user id here

            var eventEntity = newEvent.Adapt<Event>();

            return await _eventRepository.UpdateAsync(eventEntity);
        }


    }
}
