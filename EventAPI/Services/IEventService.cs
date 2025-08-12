using EventAPI.Models;
using EventAPI.Models.DTOs;

namespace EventAPI.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAll();
        Task<IEnumerable<EventDto>> GetByUserId(Guid userId);
        Task<Event> GetById(int id);
        Task<EventDto> AddParticipant(int eventId, Guid userId);
        Task<EventDto> RemoveParticipant(int eventId, Guid userId);
        Task<EventDto> Create(CreateEventDto newEvent, Guid userId);
        Task<EventDto> Update(UpdateEventDto newEvent, Guid userId);
        Task Delete(Event newEvent);
    }
}
