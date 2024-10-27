using Microsoft.EntityFrameworkCore;
using AccessAuthAPI.Data;
using AccessAuthAPI.Models;
using AccessAuthAPI.Repositories;

namespace UserAuthAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username) =>
            await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();

        public async Task<User> GetUserByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
