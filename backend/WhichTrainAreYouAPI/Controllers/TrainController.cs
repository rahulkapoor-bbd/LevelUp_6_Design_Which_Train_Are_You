using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WhichTrainAreYouAPI.DataAccess;

namespace WhichTrainAreYouAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrainController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TrainController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetTrains()
        {
            var trains = _dbContext.Trains.ToList();
            return Ok(trains);
        }

        [HttpGet("{id}")]
        public IActionResult GetTrainById(int id)
        {
            var train = _dbContext.Trains.FirstOrDefault(t => t.TrainId == id);
            if (train == null)
            {
                return NotFound();
            }

            return Ok(train);
        }

        // Other actions, such as adding, updating, or deleting trains can be added here
    }
}
