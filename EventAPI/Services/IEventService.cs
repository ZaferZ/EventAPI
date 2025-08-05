using EventAPI.Models;

namespace EventAPI.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventGetDTO>> GetAll();
        Task<IEnumerable<EventGetDTO>> GetByUserId(Guid userId);
        Task<Event> GetById(int id);
        Task<Event> Create(EventCreateDTO newEvent);
        Task<Event> Update(EventUpdateDTO newEvent);
        Task Delete(Event newEvent);
    }
}
