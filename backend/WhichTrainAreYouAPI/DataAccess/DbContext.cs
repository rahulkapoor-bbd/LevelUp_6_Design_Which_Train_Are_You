using Microsoft.EntityFrameworkCore;
using WhichTrainAreYouAPI.Models;

namespace WhichTrainAreYouAPI.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Train> Trains { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure any additional model constraints or relationships here
        }
    }
}
