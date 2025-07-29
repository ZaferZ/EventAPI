using EventAPI.Models;

namespace EventAPI.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task<Event> CreateAsync(EventCreateDTO newEvent);
        Task<Event> UpdateAsync(EventUpdateDTO newEvent);
        Task DeleteAsync(Event newEvent);
    }
}
