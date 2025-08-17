using EventAPI.Data;
using EventAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Repositories
{
    public class CommentRepository
    {
        private readonly EventDbContext _context;
        private readonly IUserRepository _userRepository;
        public CommentRepository(EventDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<Comment>> GetCommentsByEventId(int eventId)
        {
            return await _context.Comments
                .Where(c => c.EventId == eventId)
                .Include(c => c.UserId)
                .ToListAsync();
        }
    }
}
