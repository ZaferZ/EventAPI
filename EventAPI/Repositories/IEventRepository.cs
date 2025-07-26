using EventAPI.Models;

namespace EventAPI.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(Guid id);
        Task<Event> CreateAsync(Event newEvent);
        Task<Event> UpdateAsync(Event newEvent);
        Task DeleteAsync(Event newEvent);


    }
}
