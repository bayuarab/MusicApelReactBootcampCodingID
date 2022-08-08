namespace SecondV.Models
{
  public class Cart
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}