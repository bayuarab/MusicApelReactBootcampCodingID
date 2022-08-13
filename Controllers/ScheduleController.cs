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
        [HttpGet]
        public async Task<ActionResult<List<Schedule>>> GetSchedule()
        {
            return Ok(await this.dataContext.Schedules.ToListAsync());
        }
        [HttpPost]
        public async Task<ActionResult<List<Schedule>>> PostSchedule([FromBody] Schedule schedule)
        {
            var validSchedule = await this.dataContext.Schedules.FirstOrDefaultAsync(data => data.jadwal == schedule.jadwal);
            if (validSchedule != null)
                return BadRequest("Jadwal sudah ada");

            this.dataContext.Schedules.Add(schedule);
            await this.dataContext.SaveChangesAsync();

            return Ok("Jadwal berhasil ditambahkan");
        }
        [HttpDelete]
        public async Task<ActionResult<List<Schedule>>> Delete(int id)
        {
            var schedule = await this.dataContext.Schedules.FindAsync(id);
            if (schedule == null)
                return BadRequest("Not Found");

            this.dataContext.Schedules.Remove(schedule);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Courses.ToListAsync());
        }

    }
}
