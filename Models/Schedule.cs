namespace SecondV.Models
{
    public class Schedule
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        public string? jadwal { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}