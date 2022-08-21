using Microsoft.AspNetCore.Authorization;
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
            try
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
                    result.c.Price,
                    result.cc.Category,
                    courseCategoryId = result.cc.Id
                }).ToListAsync();

                return Ok(data);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
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
            try
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
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("AdminDialog")]
        public async Task<ActionResult<List<Course>>> GetCourseId()
        {
            try
            {
                var result = await this.dataContext.Courses.Select(result => new
                {
                    result.Id,
                    result.CourseTitle,
                }).ToListAsync();

                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Course>>> AddCourse(Course course)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validTitle = await this.dataContext.Courses.FirstOrDefaultAsync(data => data.CourseTitle == course.CourseTitle);
                if (validTitle != null)
                    return BadRequest("Course sudah ada");

                var validCategory = await this.dataContext.CourseCategories.FindAsync(course.CourseCategoryId);
                if (validCategory == null)
                    return BadRequest("Kategori tidak tersedia");

                this.dataContext.Courses.Add(course);
                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return Ok("Course berhasil ditambahkan");
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("categoryId/{courseCategoryId}")]
        public async Task<ActionResult<List<Course>>> GetCourseByCategoryId(int courseCategoryId)
        {
            try
            {
                var course = await this.dataContext.Courses.Where(result => result.CourseCategoryId == courseCategoryId).ToListAsync();
                if (course.Count == 0)
                    return BadRequest("Not Found");
                return Ok(course);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("categoryId/{courseCategoryId}/{Id}")]
        public async Task<ActionResult<List<Course>>> GetCourseByCategoryId(int courseCategoryId, int Id)
        {
            try
            {
                var course = await this.dataContext.Courses.Where(result => result.CourseCategoryId == courseCategoryId && result.Id != Id).ToListAsync();
                if (course.Count == 0)
                    return BadRequest("Not Found");
                return Ok(course);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPut, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Course>>> Update(Course request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var course = await this.dataContext.Courses.FindAsync(request.Id);
                if (course == null)
                    return NotFound("Course not found");

                var usedCourse = await this.dataContext.UserCourses.FirstOrDefaultAsync(data => data.CourseId == request.Id);
                if (usedCourse != null)
                    return BadRequest("Cannot edit used course");

                var validCategory = await this.dataContext.CourseCategories.FindAsync(course.CourseCategoryId);
                if (validCategory == null)
                    return BadRequest("Kategori tidak tersedia");
                course.Id = request.Id;
                course.CourseTitle = request.CourseTitle;
                course.CourseImage = request.CourseImage;
                course.CourseDesc = request.CourseDesc;
                course.Price = request.Price;
                course.CourseCategoryId = request.CourseCategoryId;

                await this.dataContext.SaveChangesAsync();

                var valid = await this.dataContext.Courses.Where(result => result.CourseTitle == request.CourseTitle).ToListAsync();
                if (valid.Count > 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    return BadRequest("Kursus dengan judul serupa ditemukan");
                }

                await dbContextTransaction.CommitAsync();

                return Ok(await this.dataContext.Courses.ToListAsync());
            }
            catch
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
                var course = await this.dataContext.Courses.FindAsync(id);
                if (course == null)
                    return BadRequest("Not Found");

                var usedCourse = await this.dataContext.UserCourses.FirstOrDefaultAsync(data => data.CourseId == id);
                if (usedCourse != null)
                    return BadRequest("Cannot delete used course");

                this.dataContext.Courses.Remove(course);
                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return Ok(await this.dataContext.Courses.ToListAsync());
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }
        }
    }
}