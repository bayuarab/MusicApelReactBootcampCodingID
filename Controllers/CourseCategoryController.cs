using Microsoft.AspNetCore.Authorization;
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
            try
            {
                return Ok(await this.dataContext.CourseCategories.ToListAsync());
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
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

        [HttpGet("Footer")]
        public async Task<ActionResult<List<CourseCategory>>> GetFooterCourseCategory()
        {
            try
            {
                var result = await this.dataContext.CourseCategories.Select(result => new
                {
                    result.Id,
                    result.Category,
                }).ToListAsync();

                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<CourseCategory>>> AddCourseCategory([FromBody] CourseCategory courseCategory)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validCategory = await this.dataContext.CourseCategories.FirstOrDefaultAsync(data => data.Category == courseCategory.Category);
                if (validCategory != null)
                    return BadRequest("Category sudah ada");

                this.dataContext.CourseCategories.Add(courseCategory);
                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return Ok("Category berhasil ditambahkan");
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPut, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<CourseCategory>>> Update(CourseCategory request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var courseCat = await this.dataContext.CourseCategories.FindAsync(request.Id);
                if (courseCat == null)
                    return BadRequest("Category not found");

                var usedCategory = await this.dataContext.UserCourses.
                    Join(this.dataContext.Courses,
                        uc => uc.CourseId,
                        c => c.Id,
                        (ca, c) => new { ca, c }).
                    Where(data => data.c.CourseCategoryId == request.Id).
                    ToListAsync();
                    
                if (usedCategory.Count != 0)
                    return BadRequest("Cannot edit used category");

                courseCat.Category = request.Category;
                courseCat.image = request.image;
                courseCat.desc = request.desc;

                await this.dataContext.SaveChangesAsync();

                var valid = await this.dataContext.CourseCategories.
                Where(result => result.Category == request.Category).ToListAsync();
                if (valid.Count > 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    return BadRequest("Failed (Rollback)");
                }

                await dbContextTransaction.CommitAsync();

                return Ok(await this.dataContext.CourseCategories.ToListAsync());
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }

        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<List<CourseCategory>>> Delete(int id)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var courseCategory = await this.dataContext.CourseCategories.FindAsync(id);
                if (courseCategory == null)
                    return BadRequest("Not Found");

                var usedCategory = await this.dataContext.UserCourses.
                    Join(this.dataContext.Courses,
                        uc => uc.CourseId,
                        c => c.Id,
                        (ca, c) => new { ca, c }).
                    Where(data => data.c.CourseCategoryId == id).
                    ToListAsync();
                    
                if (usedCategory.Count != 0)
                    return BadRequest("Cannot edit used category");

                this.dataContext.CourseCategories.Remove(courseCategory);
                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return Ok(await this.dataContext.CourseCategories.ToListAsync());
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }
        }
    }
}