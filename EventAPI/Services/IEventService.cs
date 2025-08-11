using EventAPI.Models;

namespace EventAPI.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventGetDTO>> GetAll();
        Task<IEnumerable<EventGetDTO>> GetByUserId(Guid userId);
        Task<Event> GetById(int id);
        Task<Event> AddParticipant(int eventId, Guid userId);
        Task<Event> Create(EventCreateDTO newEvent, Guid userId);
        Task<Event> Update(EventUpdateDTO newEvent, Guid userId);
        Task Delete(Event newEvent);
    }
}
