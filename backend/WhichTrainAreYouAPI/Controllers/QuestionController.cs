using Microsoft.AspNetCore.Mvc;
using WhichTrainAreYouAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhichTrainAreYouAPI.Models;

namespace WhichTrainAreYouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("quiz")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            var questions = await _dbContext.Questions.ToListAsync();
            return Ok(questions);
        }
    }
}
