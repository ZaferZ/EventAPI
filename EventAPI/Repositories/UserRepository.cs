using EventAPI.Data;
using EventAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EventDbContext _context;
        public UserRepository(EventDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserById(Guid userId)
        {

            var user = await _context.Users
            .Where(u => u.Id == userId)
             .Select(u => new User
             {
                 Id = u.Id,
                 Username = u.Username,
                 FirstName = u.FirstName,
                 LastName = u.LastName,
                 Email = u.Email
             })
                .FirstOrDefaultAsync();
            return user;
        }

    }
}
