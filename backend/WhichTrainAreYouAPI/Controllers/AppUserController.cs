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
    private readonly JWTHelper _jwtHelper;

    public AppUserController(ApplicationDbContext dbContext, JWTHelper jwtHelper)
    {
      _dbContext = dbContext;
      _jwtHelper = jwtHelper;
    }


    [HttpPost("register")]
    public IActionResult Register(UserRegistrationDTO registrationDto)
    {
      if (_dbContext.AppUsers.Any(u => u.Username == registrationDto.Username))
      {
        return BadRequest("Username already exists.");
      }

      string salt;
      string hashedPassword = PasswordHelper.HashPassword(registrationDto.Password, out salt);

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

      return Ok(user);
    }

    [HttpGet("data")]
    public IActionResult GetUser(String username)
    {
      var user = _dbContext.AppUsers.FirstOrDefault(u => u.Username == username);

      if (user == null)
      {
        return NotFound("User not found");
      }

      return Ok(user);
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
