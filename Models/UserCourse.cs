namespace SecondV.Models
{
    public class UserCourse
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}