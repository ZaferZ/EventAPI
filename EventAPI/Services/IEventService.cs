using EventAPI.Models;
using EventAPI.Models.DTOs;

namespace EventAPI.Services
{
    public interface IEventService
    {
        /// <summary>
        /// Retrieves all events from the repository and maps them to a list of <see cref="EventDto"/> objects.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.  
        /// The task result contains an <see cref="IEnumerable{EventDto}"/> with all events.
        /// </returns>
        Task<IEnumerable<EventDto>> GetAll();

        /// <summary>
        /// Retrieves all events created by a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose events are being retrieved.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains a collection of <see cref="EventDto"/> objects belonging to the user.
        /// </returns>
        Task<IEnumerable<EventDto>> GetByUserId(Guid userId);

        /// <summary>
        /// Retrieves a specific event by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the event.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the <see cref="EventDto"/> if found; otherwise <c>null</c>.
        /// </returns>
        Task<EventDto> GetById(int id);

        /// <summary>
        /// Adds a participant to an existing event.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event.</param>
        /// <param name="userId">The unique identifier of the user to add as a participant.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the updated <see cref="EventDto"/>.
        /// </returns>
        /// <exception cref="Exception">Thrown when the event or user is not found, or the user is already a participant.</exception>
        Task<EventDto> AddParticipant(int eventId, Guid userId);

        /// <summary>
        /// Removes a participant from an existing event.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event.</param>
        /// <param name="userId">The unique identifier of the user to remove.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the updated <see cref="EventDto"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the event or user is not found,  
        /// or the user is not a participant.
        /// </exception>
        Task<EventDto> RemoveParticipant(int eventId, Guid userId);

        /// <summary>
        /// Creates a new event for the specified user.
        /// </summary>
        /// <param name="newEvent">The event details to create.</param>
        /// <param name="userId">The unique identifier of the user creating the event.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the newly created <see cref="EventDto"/>.
        /// </returns>
        Task<EventDto> Create(CreateEventDto newEvent, Guid userId);

        /// <summary>
        /// Updates the details of an existing event.
        /// </summary>
        /// <param name="newEvent">The updated event data.</param>
        /// <param name="userId">The unique identifier of the user performing the update.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the updated <see cref="EventDto"/>.
        /// </returns>
        Task<EventDto> Update(UpdateEventDto newEvent, Guid userId);

        /// <summary>
        /// Deletes an event if the current user is allowed.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the event does not exist.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the current user is not the owner or an admin.
        /// </exception>
        Task Delete(int eventId);
    }
}
