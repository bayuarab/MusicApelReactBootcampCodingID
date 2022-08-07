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
            return Ok(await this.dataContext.Carts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Cart>>> Get(int id)
        {
            var CartID = await this.dataContext.Carts.FindAsync(id);
            if (CartID == null)    
                return BadRequest("Not Found");

            return Ok(CartID);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Cart>>> Delete(int id)
        {
            var userCart = await this.dataContext.Carts.FindAsync(id);
            if (userCart == null)
                return BadRequest("Not Found");

            this.dataContext.Carts.Remove(userCart);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Carts.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Cart>>> AddUserCart(Cart cart)
        {
            var existedCart = this.dataContext.Carts.Where(data => data.UserId == cart.UserId);
            var statusExist = existedCart.FirstOrDefault(x => x.CourseId == cart.CourseId);
            if (statusExist != null)
                return BadRequest("Course already added to Cart by UID " + cart.UserId.ToString());

            this.dataContext.Carts.Add(cart);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Carts.ToListAsync());
        }
    }
}