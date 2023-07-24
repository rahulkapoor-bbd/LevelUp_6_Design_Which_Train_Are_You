namespace WhichTrainAreYouAPI.Models
{
    public class AppUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public int Score { get; set; }
        public int TrainId { get; set; }
    }

}
