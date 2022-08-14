using System.ComponentModel.DataAnnotations.Schema;

namespace SecondV.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? CourseTitle { get; set; }
        public string? CourseImage { get; set; }
        public string? CourseDesc { get; set; }
        public int Price { get; set; }

        [ForeignKey("CourseCategory")]
        public int? CourseCategoryId { get; set; }
    }
}