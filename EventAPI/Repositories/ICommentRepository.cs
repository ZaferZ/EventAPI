using EventAPI.Models;

namespace EventAPI.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByEventId(int eventId);
    }
}
