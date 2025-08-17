using EventAPI.Models;

namespace EventAPI.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation.  
        /// The task result contains the <see cref="User"/> if found; otherwise <c>null</c>.
        /// </returns>
        Task<User> GetUserById(Guid userId);
    }
}
