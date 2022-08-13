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

        [HttpGet]
        public async Task<ActionResult<List<Cart>>> GetAllCart()
        {
            try
            {
                return Ok(await this.dataContext.Carts.ToListAsync());
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Cart>>> Get(int id)
        {
            try
            {
                var CartID = await this.dataContext.Carts.FindAsync(id);
                if (CartID == null)    
                    return BadRequest("Not Found");

                return Ok(CartID);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Cart>>> Delete(int id)
        {
            try
            {
                var userCart = await this.dataContext.Carts.FindAsync(id);
                if (userCart == null)
                    return BadRequest("Not Found");

                this.dataContext.Carts.Remove(userCart);
                await this.dataContext.SaveChangesAsync();

                return Ok(await this.dataContext.Carts.ToListAsync());
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }    
        }

        [HttpPost]
        public async Task<ActionResult<List<Cart>>> AddUserCart(Cart cart)
        {
            
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

                return Ok(await this.dataContext.Carts.ToListAsync());
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
            
        }
    }
}