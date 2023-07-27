using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WhichTrainAreYouAPI.DataAccess;
using WhichTrainAreYouAPI.Models;
using WhichTrainAreYouAPI.Utils;

namespace WhichTrainAreYouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JWTHelper _jwtHelper;

        public AppUserController(ApplicationDbContext dbContext, JWTHelper jwtHelper)
        {
            _dbContext = dbContext;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegistrationDTO registrationDto)
        {
            // Validate inputs and check if the username already exists
            if (_dbContext.AppUsers.Any(u => u.Username == registrationDto.Username))
            {
                return BadRequest("Username already exists.");
            }

            // Generate salt and hash the password
            string salt;
            string hashedPassword = PasswordHelper.HashPassword(registrationDto.Password, out salt);

            // Save the user to the database
            var newUser = new AppUser
            {
                Username = registrationDto.Username,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            _dbContext.AppUsers.Add(newUser);
            _dbContext.SaveChanges();

            return Ok("User registered successfully!");
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO loginDto)
        {
            var user = _dbContext.AppUsers.FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash, user.Salt))
            {
                return BadRequest("Invalid credentials");
            }

            var tokenString = _jwtHelper.GenerateJWTToken(user);

            Response.Headers.Add("Authorization", "Bearer " + tokenString);

            return NoContent();
        }
    }
}
