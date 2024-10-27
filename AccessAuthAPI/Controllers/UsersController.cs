using AccessAuthAPI.DTOs;
using AccessAuthAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessAuthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET /users: Retrieve all users (Admin-only access)
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(u => new UserProfile
            {
                ID = u.Id,
                Username = u.Username,
                Role = u.Role
            });
            return Ok(userDtos);
        }

        // PUT /users/{id}: Update a specific user (self or Admin)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, UserProfile updateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id); 
            if (user == null)
                return NotFound("User not found.");

            var currentUser = User.Identity.Name;

            if (user.Username != currentUser && !User.IsInRole("Admin"))
                return Forbid();

            user.Username = updateDto.Username ?? user.Username;
            user.Role = updateDto.Role ?? user.Role;

            await _userRepository.UpdateUserAsync(user); 
            return NoContent();
        }

        // DELETE /users/{id}: Delete a specific user (Admin-only access)
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id); 
            if (user == null)
                return NotFound("User not found.");

            await _userRepository.DeleteUserAsync(user); 
            return NoContent();
        }
    }
}
