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
            modelBuilder.Entity<Train>().ToTable("Train", schema: "dbo");
            modelBuilder.Entity<Question>().ToTable("Question", schema: "dbo");
            modelBuilder.Entity<AppUser>().ToTable("AppUser", schema: "dbo");

            // Define the primary key for the Train entity
            modelBuilder.Entity<Train>().HasKey(t => t.TrainId);

            // Define the primary key for the Question entity
            modelBuilder.Entity<Question>().HasKey(q => q.QuestionId);

            // Define the primary key for the AppUser entity
            modelBuilder.Entity<AppUser>().HasKey(u => u.UserId);
        }
    }
}
