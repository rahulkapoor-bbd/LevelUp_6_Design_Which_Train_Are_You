namespace WhichTrainAreYouAPI.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int TrainId { get; set; }

        public Train Train { get; set; }
    }

}
