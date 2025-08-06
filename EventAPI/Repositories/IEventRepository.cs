using EventAPI.Models;

namespace EventAPI.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAll();
        Task<IEnumerable<Event>> GetByUserId(Guid userId);
        Task<Event> GetById(int id);
        Task<Event> Create(Event newEvent);
        Task<Event> Update(Event newEvent);
        Task Delete(Event newEvent);


    }
}
