namespace SecondV.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? CourseTitle { get; set; }
        public string? CourseImage { get; set; }
        public string? CourseDesc { get; set; }
        public string? Jadwal { get; set; }
        public int Price { get; set; }
        public int CourseCategoryId { get; set; }
        public CourseCategory? CourseCategory { get; set; }
    }
}