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

        [HttpPost]
        public async Task<ActionResult<List<CourseCategory>>> AddCourseCategory(CourseCategory courseCategory)
        {
            this.dataContext.CourseCategories.Add(courseCategory);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.CourseCategories.ToListAsync());
        }
    }
}