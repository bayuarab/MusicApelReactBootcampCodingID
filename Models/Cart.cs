namespace SecondV.Models
{
  public class Cart
    {
        public int Id { get; set; }
        public int FKCourse { get; set; }
        public int UserId { get; set; }
    }
}