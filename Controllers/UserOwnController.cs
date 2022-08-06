using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
  [ApiController]
    [Route("api/[controller]")]
    public class UserOwnController : ControllerBase
    {
        private DataContext dataContext;

        public UserOwnController(DataContext dataContext)
        {
            this.dataContext = dataContext;           
        }

        [HttpGet("Cart")]
        public async Task<ActionResult<List<MasterInvoice>>> GetAllCart()
        {
            return Ok(await this.dataContext.Carts.ToListAsync());
        }

        [HttpGet("Cart/{userID}")]
        public async Task<ActionResult<Cart>> GetCartByUID(int userID)
        {
            var userCart = await this.dataContext.Carts.Where(data => data.UserId == userID).ToListAsync();
            if (userCart.Count == 0)
                return BadRequest("Not Found");
            return Ok(userCart);
        }

        [HttpDelete("Cart/{id}")]
        public async Task<ActionResult<List<Cart>>> Delete(int id)
        {
            var userCart = await this.dataContext.Carts.FindAsync(id);
            if (userCart == null)
                return BadRequest("Not Found");

            this.dataContext.Carts.Remove(userCart);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Carts.ToListAsync());
        }

        [HttpPost("Cart")]
        public async Task<ActionResult<List<Cart>>> AddUserCart(Cart cart)
        {
            var existedCart = this.dataContext.Carts.Where(data => data.UserId == cart.UserId);
            var statusExist = existedCart.FirstOrDefault(x => x.FKCourse == cart.FKCourse);
            if (statusExist != null)
                return BadRequest("Course already added to Cart");

            this.dataContext.Carts.Add(cart);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Carts.ToListAsync());
        }
    }
}