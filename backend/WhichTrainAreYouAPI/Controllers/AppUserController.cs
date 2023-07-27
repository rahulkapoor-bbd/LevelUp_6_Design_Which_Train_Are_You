using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public AppUserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegistrationDTO registrationDto)
        {
            // Validate inputs and check if the username already exists
            // Generate salt and hash the password
            string salt;
            string hashedPassword = PasswordHelper.HashPassword(registrationDto.Password, out salt);

            // Save the user to the database
            var newUser = new AppUser
            {
                Username = registrationDto.Username,
                PasswordHash = hashedPassword,
                Salt = salt,
                Score = 0
            };

            _dbContext.AppUsers.Add(newUser);
            _dbContext.SaveChanges();

            return Ok("User registered successfully!");
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO loginDto)
        {
            // Find the user in the database based on the provided username
            var user = _dbContext.AppUsers.FirstOrDefault(u => u.Username == loginDto.Username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Verify the password
            if (!PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash, user.Salt))
            {
                return BadRequest("Invalid credentials");
            }

            // You can generate a JWT token and return it to the client for further authentication

            // For simplicity, let's just return an "Authenticated" message here
            return Ok("Authenticated!");
        }

        [HttpPut("updateTrainId")]
        public async Task<IActionResult> UpdateUserTrainId(UserTrainUpdateDTO userTrainUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input data.");
            }

            var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Username == userTrainUpdateDto.Username);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.TrainId = userTrainUpdateDto.NewTrainId;
            _dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Failed to update the user. Please try again.");
            }

            return Ok("User TrainId updated successfully.");
        }
    }
}
