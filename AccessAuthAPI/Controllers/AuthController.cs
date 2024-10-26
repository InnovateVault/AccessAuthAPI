using AccessAuthAPI.DTOs;
using AccessAuthAPI.Models;
using AccessAuthAPI.Repositories;
using AccessAuthAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessAuthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister dto)
        {
            if (await _userRepository.GetUserByUsernameAsync(dto.Username) != null)
                return BadRequest("Username already exists.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
                Role = dto.Role ?? "User"
            };

            await _userRepository.AddUserAsync(user);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin dto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = _tokenService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}