namespace SecondV.Models
{
  public class Cart
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}