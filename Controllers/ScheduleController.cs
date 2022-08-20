using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        private DataContext dataContext;

        public ScheduleController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet("Admin"), Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Schedule>>> Get()
        {
            try
            {
                var data = await this.dataContext.Schedules.
                Join(this.dataContext.Courses,
                    s => s.CourseId,
                    c => c.Id,
                    (s, c) => new { s, c }).
                OrderBy(result => result.s.CourseId).
                Select(result => new
                {
                    result.c.CourseTitle,
                    result.s.jadwal,
                    result.s.id,
                    result.s.CourseId
                }).ToListAsync();

                return Ok(data);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("ByCourseId/{courseId}")]
        public async Task<ActionResult<List<Schedule>>> GetScheduleByCourseId(int courseId)
        {
            try
            {
                var data = await this.dataContext.Schedules.Where(data => data.CourseId == courseId).ToListAsync();
                if (data.Count == 0)
                    return NotFound("Not Found");

                return Ok(data);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Schedule>>> PostSchedule([FromBody] Schedule schedule)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validCourse = await this.dataContext.Courses.FindAsync(schedule.CourseId);
                if (validCourse == null)
                    return NotFound("Kelas tidak ditemukan");

                var validSchedule = await this.dataContext.Schedules.Where(data => data.CourseId == schedule.CourseId).FirstOrDefaultAsync(data => data.jadwal == schedule.jadwal);
                if (validSchedule != null)
                    return BadRequest("Jadwal sudah ada");

                this.dataContext.Schedules.Add(schedule);
                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                var result = new
                {
                    message = "Jadwal berhasil ditambahkan pada kelas",
                    course = validCourse.CourseTitle
                };

                return Ok(result);
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPut, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Schedule>>> Update(Schedule request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var schedule = await this.dataContext.Schedules.FindAsync(request.id);
                if (schedule == null)
                    return BadRequest("Schedule not found");

                var usedSchedule = await this.dataContext.UserCourses.FirstOrDefaultAsync(data => data.ScheduleId == request.id);
                if (usedSchedule != null)
                    return BadRequest("Cannot edit used schedule");

                schedule.jadwal = request.jadwal;
                schedule.CourseId = request.CourseId;

                await this.dataContext.SaveChangesAsync();

                var valid = await this.dataContext.Schedules.
                Where(result => result.jadwal == request.jadwal && result.CourseId == request.CourseId).ToListAsync();
                if (valid.Count > 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    return BadRequest("Failed (Rollback)");
                }

                await dbContextTransaction.CommitAsync();

                return Ok(await this.dataContext.Schedules.ToListAsync());
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<List<Schedule>>> Delete(int id)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var schedule = await this.dataContext.Schedules.FindAsync(id);
                if (schedule == null)
                    return NotFound("Not Found");

                var usedSchedule = await this.dataContext.UserCourses.FirstOrDefaultAsync(data => data.ScheduleId == id);
                if (usedSchedule != null)
                    return BadRequest("Cannot delete used schedule");

                this.dataContext.Schedules.Remove(schedule);
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
