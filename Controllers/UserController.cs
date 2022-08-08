using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private DataContext dataContext;

        public UserController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUser()
        {
            return Ok(await this.dataContext.Users.ToListAsync());
        }

        [HttpPost("Login")]
        public async Task<ActionResult<List<User>>> UserLogin(User request)
        {
            var userValid = await this.dataContext.Users.FirstOrDefaultAsync(result => result.email == request.email);
            if (userValid == null)
                return BadRequest("username not found");
            if (userValid.password != request.password)
                return BadRequest("Wrong Password");

            return Ok(userValid);
        }

        [HttpPost("CheckEmail")]
        public async Task<ActionResult<List<User>>> CheckEmail(User request)
        {
            var checkEmail = await this.dataContext.Users.FirstOrDefaultAsync(result => result.email == request.email);
            if (checkEmail == null)
            {
                return Ok("Email Ok");
            }

            return BadRequest("Registration Failed");
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUsers(User user)
        {
            this.dataContext.Users.Add(user);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Users.ToListAsync());
        }

        [HttpGet("Cart/{userID}")]
        public async Task<ActionResult<Cart>> GetCartByUID(int userID)
        {
            var userCart = await this.dataContext.Carts.
            Join(this.dataContext.Courses,
                ca => ca.CourseId,
                c => c.Id,
                (ca, c) => new { ca, c }).
            Join(this.dataContext.CourseCategories,
                cac => cac.c.CourseCategoryId,
                cc => cc.Id,
                (caccc, cc) => new { caccc, cc }).
            Where(data => data.caccc.ca.UserId == userID).
            Select(result => new
            {
                Course = result.caccc.c.CourseTitle,
                Category = result.cc.Category,
                Schedule = result.caccc.c.Jadwal,
                Price = result.caccc.c.Price,
                Id = result.caccc.ca.Id
            }).
            ToListAsync();

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

        [HttpGet("Invoices/{userId}")]
        public async Task<ActionResult<MasterInvoice>> GetByUserID(int userId)
        {
            var masterInvoice = await this.dataContext.MasterInvoices.Where(data => data.UserId == userId).ToListAsync();
            if (masterInvoice.Count == 0)
                return BadRequest("Not Found");
            return Ok(masterInvoice);
        }

        [HttpGet("Courses/{userId}")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByUserID(int userId)
        {
            var courseData = await this.dataContext.MasterInvoices.
            Join(this.dataContext.InvoiceDetails,
                mi => mi.Id,
                ind => ind.MasterInvoiceId,
                (mi, ind) => new { mi, ind }).
            Join(this.dataContext.Courses,
                mind => mind.ind.CourseId,
                c => c.Id,
                (mindc, c) => new { mindc, c }).
            Join(this.dataContext.CourseCategories,
                mindc => mindc.c.CourseCategoryId,
                cc => cc.Id,
                (mindccc, cc) => new { mindccc, cc }).
            Where(result => result.mindccc.mindc.mi.UserId == userId).
            Select(result => new
            {
                Course = result.mindccc.c.CourseTitle,
                Category = result.cc.Category,
                Schedule = result.mindccc.c.Jadwal
            }).ToListAsync();

            if (courseData.Count == 0)
                return BadRequest("Not Found");
            return Ok(courseData);
        }
    }
}