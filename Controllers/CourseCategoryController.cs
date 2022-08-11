using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseCategoryController : ControllerBase
    {
        private DataContext dataContext;

        public CourseCategoryController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseCategory>>> Get()
        {
            return Ok(await this.dataContext.CourseCategories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseCategory>> GetCourseCategoryById(int id)
        {
            try
            {
                var courseCategory = await this.dataContext.CourseCategories.FindAsync(id);
                if (courseCategory == null)
                    return BadRequest("Not Found");
                return Ok(courseCategory);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<CourseCategory>>> AddCourseCategory(CourseCategory courseCategory)
        {
            var validCategory = await this.dataContext.CourseCategories.FirstOrDefaultAsync(data => data.Category == courseCategory.Category);
            if (validCategory != null)
                return BadRequest("Category sudah ada");

            this.dataContext.CourseCategories.Add(courseCategory);
            await this.dataContext.SaveChangesAsync();

            return Ok("Category berhasil ditambahkan");
        }

        [HttpPut]
        public async Task<ActionResult<List<CourseCategory>>> Update(CourseCategory request)
        {
            var courseCat = await this.dataContext.CourseCategories.FindAsync(request.Id);
            if (courseCat == null)
                return BadRequest("User not found");

            courseCat.Category = request.Category;
            courseCat.image = request.image;
            courseCat.desc = request.desc;

            await this.dataContext.SaveChangesAsync();
            return Ok(await this.dataContext.CourseCategories.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<CourseCategory>>> Delete(int id)
        {
            var courseCategory = await this.dataContext.CourseCategories.FindAsync(id);
            if (courseCategory == null)
                return BadRequest("Not Found");

            this.dataContext.CourseCategories.Remove(courseCategory);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.CourseCategories.ToListAsync());
        }
    }
}