﻿using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public async Task<ActionResult<List<Schedule>>> GetSchedule()
        {
            return Ok(await this.dataContext.Schedules.ToListAsync());
        }

        [HttpGet("Admin")]
        public async Task<ActionResult<List<Schedule>>> Get()
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

        [HttpGet("ByCourseId/{courseId}")]
        public async Task<ActionResult<List<Schedule>>> GetScheduleByCourseId(int courseId)
        {
            var data = await this.dataContext.Schedules.Where(data => data.CourseId == courseId).ToListAsync();
            if (data.Count == 0)
                return NotFound("Not Found");

            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<List<Schedule>>> PostSchedule([FromBody] Schedule schedule)
        {
            var validCourse = await this.dataContext.Courses.FindAsync(schedule.CourseId);
            if (validCourse == null)
                return NotFound("Kelas tidak ditemukan");

            var validSchedule = await this.dataContext.Schedules.Where(data => data.CourseId == schedule.CourseId).FirstOrDefaultAsync(data => data.jadwal == schedule.jadwal);
            if (validSchedule != null)
                return BadRequest("Jadwal sudah ada");

            this.dataContext.Schedules.Add(schedule);
            await this.dataContext.SaveChangesAsync();

            var result = new {
                message = "Jadwal berhasil ditambahkan pada kelas",
                course = validCourse.CourseTitle
            };

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Schedule>>> Delete(int id)
        {
            var schedule = await this.dataContext.Schedules.FindAsync(id);
            if (schedule == null)
                return NotFound("Not Found");

            var usedSchedule = await this.dataContext.UserCourses.FirstOrDefaultAsync(data => data.ScheduleId == id);
            if (usedSchedule != null)
                return BadRequest("Cannot delete used schedule");

            this.dataContext.Schedules.Remove(schedule);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Courses.ToListAsync());
        }

    }
}
