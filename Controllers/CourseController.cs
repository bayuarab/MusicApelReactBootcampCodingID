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
                    (c, cc) => new { c, cc }).
                Select(result => new
                {
                    result.c.Id,
                    result.c.CourseTitle,
                    result.c.Jadwal,
                    result.c.Price,
                    result.cc.Category,
                    courseCategoryId = result.cc.Id
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            try
            {
                var course = await this.dataContext.Courses.FindAsync(id);
                if (course == null)
                    return BadRequest("Not Found");
                return Ok(course);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("LandingPage")]
        public async Task<ActionResult<List<Course>>> GetClass()
        {
            var data = await this.dataContext.Courses.
                Join(this.dataContext.CourseCategories,
                    c => c.CourseCategoryId,
                    cc => cc.Id,
                    (c, cc) => new { c, cc }).
                OrderByDescending(result => result.c.Id).
                Select(result => new
                {
                    result.c.Id,
                    result.c.CourseTitle,
                    result.c.CourseImage,
                    result.c.Price,
                    result.cc.Category,
                    CategoryId = result.cc.Id 
                }).ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<List<Course>>> AddCourse(Course course)
        {
            var validTitle = await this.dataContext.Courses.FirstOrDefaultAsync(data => data.CourseTitle == course.CourseTitle);
            if (validTitle != null)
                return BadRequest("Course sudah ada");

            this.dataContext.Courses.Add(course);
            await this.dataContext.SaveChangesAsync();

            return Ok("Course berhasil ditambahkan");
        }

        [HttpGet("categoryId/{courseCategoryId}")]
        public async Task<ActionResult<List<Course>>> GetCourseByCategoryId(int courseCategoryId)
        {
            try
            {
                var course = await this.dataContext.Courses.Where(result => result.CourseCategoryId == courseCategoryId).ToListAsync();
                if (course.Count == null)
                    return BadRequest("Not Found");
                return Ok(course);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPut]
        public async Task<ActionResult<List<Course>>> Update(Course request)
        {
            var course = await this.dataContext.Courses.FindAsync(request.Id);
            if (course == null)
                return BadRequest("User not found");

            course.CourseTitle = request.CourseTitle;
            course.CourseImage = request.CourseImage;
            course.CourseDesc = request.CourseDesc;
            course.Jadwal = request.Jadwal;
            course.Price = request.Price;
            course.CourseCategoryId = request.CourseCategoryId;

            await this.dataContext.SaveChangesAsync();
            return Ok(await this.dataContext.Courses.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<CourseCategory>>> Delete(int id)
        {
            var course = await this.dataContext.Courses.FindAsync(id);
            if (course == null)
                return BadRequest("Not Found");

            this.dataContext.Courses.Remove(course);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Courses.ToListAsync());
        }
    }
}