using AccessAuthAPI.Models;

namespace AccessAuthAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task DeleteUserAsync(User user);
        Task SaveChangesAsync();
    }
}
