using EventAPI.Models;
using EventAPI.Repositories;

namespace EventAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<Event> CreateAsync(Event newEvent)
        {//Generata iD
            return await _eventRepository.CreateAsync(newEvent);
        }

        public async Task DeleteAsync(Event newEvent)
        {
           await _eventRepository.DeleteAsync(newEvent);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
           return await _eventRepository.GetAllAsync();
        }

        public async Task<Event> GetByIdAsync(Guid id)
        {
          return await _eventRepository.GetByIdAsync(id);
        }

        public async  Task<Event> UpdateAsync(Event newEvent)
        {
           return await _eventRepository.UpdateAsync(newEvent);
        }
    }
}
