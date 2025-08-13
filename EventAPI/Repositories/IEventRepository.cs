using EventAPI.Models;
using EventAPI.Models.DTOs;

namespace EventAPI.Repositories
{
    public interface IEventRepository
    {
        /// <summary>
        /// Retrieves all events, including their participants.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains a collection of <see cref="Event"/> objects.
        /// </returns>
        Task<IEnumerable<Event>> GetAll();

        /// <summary>
        /// Retrieves all events created or owned by a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains a collection of <see cref="Event"/> objects.
        /// </returns>
        Task<IEnumerable<Event>> GetByUserId(Guid userId);

        /// <summary>
        /// Retrieves an event by its ID, including its participants.
        /// </summary>
        /// <param name="id">The unique identifier of the event.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the <see cref="Event"/> if found.
        /// </returns>
        /// <exception cref="KeyNotFoundException">Thrown when the event is not found.</exception>
        Task<Event> GetById(int id);

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the <see cref="User"/> if found; otherwise <c>null</c>.
        /// </returns>
        Task<User> GetUserById(Guid userId);

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="newEvent">The event to create.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the newly created <see cref="Event"/>.
        /// </returns>
        Task<Event> Create(Event ev);

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="ev">The updated event data.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the updated <see cref="Event"/>.
        /// </returns>
        Task<Event> Update(Event ev);

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="ev">The event to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Delete(Event ev);



    }
}
