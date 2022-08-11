namespace SecondV.Models
{
    public class CourseCategory
    {
        public int Id { get; set; }
        [Required]
        public string? Category { get; set; }
        public string? image { get; set; }
        public string? desc { get; set; }
    }
}