namespace WhichTrainAreYouAPI.Models
{
    public class Train
    {
        public int TrainId { get; set; }
        public string TrainName { get; set; }
        public string Description { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<AppUser> Users { get; set; }
    }
}
