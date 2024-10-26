using AccessAuthAPI.Models;

namespace AccessAuthAPI.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}