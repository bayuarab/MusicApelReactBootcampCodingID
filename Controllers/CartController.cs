using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private DataContext dataContext;

        public CartController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpPost, Authorize(Roles = "student")]
        public async Task<ActionResult<List<Cart>>> AddUserCart(Cart cart)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validUserId = await this.dataContext.Users.FindAsync(cart.UserId);
                if (validUserId == null)
                    return BadRequest("Not valid data");

                var validCart = await this.dataContext.Carts.Where(data => data.UserId == cart.UserId).ToListAsync();
                var statusExist = validCart.FirstOrDefault(exist => exist.CourseId == cart.CourseId);
                if ((statusExist != null))
                    return BadRequest("course sudah ditambahkan ke keranjang!");

                var validSchedule = await this.dataContext.Schedules.FindAsync(cart.ScheduleId);
                if (validSchedule == null || validSchedule.CourseId != cart.CourseId)
                    return BadRequest("Data not valid");

                this.dataContext.Carts.Add(cart);
                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return Ok(await this.dataContext.Carts.ToListAsync());
            }
            catch
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }

        }
    }
}