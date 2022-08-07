using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
  [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private DataContext dataContext;

        public CourseController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Course>>> Get()
        {
            var data = await this.dataContext.Courses.
                Join(this.dataContext.CourseCategories,
                    c => c.CourseCategoryId,
                    cc => cc.Id,
                    (c, cc) => new {c, cc}).
                Select(result => new {
                    result.c.Id,
                    result.c.CourseTitle,
                    result.c.Jadwal,
                    result.c.Price,
                    result.cc.Category
                }).ToListAsync();
            
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<List<Course>>> AddCourse(Course course)
        {
            this.dataContext.Courses.Add(course);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Courses.ToListAsync());
        }
    }
}