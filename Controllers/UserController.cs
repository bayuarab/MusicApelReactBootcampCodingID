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

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUsers(User user)
        {
            this.dataContext.Users.Add(user);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.Users.ToListAsync());
        }

        [HttpPost("Login")]
        public async Task<ActionResult<List<User>>> UserLogin(User request)
        {
            // var valid = await this.dataContext.Users.
            // Where(result => result.Username == request.Username).
            // Select(result => new {
            //     id = result.Id,
            //     roles = result.roles
            // }).ToListAsync();
            // return Ok(valid[0]);
            //------------------------------------ Output id and roles only

            var userValid = await this.dataContext.Users.FirstOrDefaultAsync(result => result.email == request.email);
            if (userValid == null)
                return BadRequest("email not found");
            // if (userValid.password != request.password) comment due to easier test purpose
            //     return BadRequest("Wrong Password");

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

        [HttpPost("MInvoice")]
        public async Task<ActionResult<List<MasterInvoice>>> AddMasterInvoice(MasterInvoice masterInvoice)
        {
            try
            {   
                var validId = await this.dataContext.MasterInvoices.FindAsync(masterInvoice.Id);
                var validNoInvoice = await this.dataContext.MasterInvoices.FirstOrDefaultAsync(data => data.NoInvoice == masterInvoice.NoInvoice);
                var validUserId = await this.dataContext.MasterInvoices.FindAsync(masterInvoice.UserId);
                if(validId != null || validNoInvoice != null || validUserId == null)
                    return BadRequest("Not valid data");

                this.dataContext.MasterInvoices.Add(masterInvoice);
                await this.dataContext.SaveChangesAsync();
                var newMasterInvoice = this.dataContext.MasterInvoices.FirstOrDefault(result => result.NoInvoice == masterInvoice.NoInvoice);
                return Ok(newMasterInvoice); 
            }

            catch 
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost("InvoiceDetails")]
        public async Task<ActionResult<List<InvoiceDetail>>> AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            try
            {
                var validMasterId = await this.dataContext.MasterInvoices.FindAsync(invoiceDetail.MasterInvoiceId);
                var validId = await this.dataContext.InvoiceDetails.FindAsync(invoiceDetail.Id);
                var validCourseId = await this.dataContext.Courses.FindAsync(invoiceDetail.CourseId);
                if ((validMasterId == null) || (validId != null) || (validCourseId == null))
                    return BadRequest("Not valid data");

                if (validMasterId.NoInvoice != invoiceDetail.NoInvoice)
                    return BadRequest("Not valid data");

                this.dataContext.InvoiceDetails.Add(invoiceDetail);
                await this.dataContext.SaveChangesAsync();

                return Ok(await this.dataContext.InvoiceDetails.Where(result => result.NoInvoice == invoiceDetail.NoInvoice).ToListAsync());    
            }
            catch
            {
                 return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("Cart/{userID}")]
        public async Task<ActionResult<Cart>> GetCartByUID(int userID)
        {
            try
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
                    CourseId = result.caccc.c.Id,
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
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpDelete("Cart/{UserId}/{id}")]
        public async Task<ActionResult<List<Cart>>> Delete(int UserId, int id)
        {
            try
            {
                var userCart = await this.dataContext.Carts.FindAsync(id);
                if (userCart == null)
                    return BadRequest("Not Found");
                
                if (userCart.UserId != UserId)
                    return BadRequest("Not valid data");

                this.dataContext.Carts.Remove(userCart);
                await this.dataContext.SaveChangesAsync();

                return Ok(await this.dataContext.Carts.ToListAsync());
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("Invoices/{userId}")]
        public async Task<ActionResult<MasterInvoice>> GetByUserID(int userId)
        {
            try
            {
                var masterInvoice = await this.dataContext.MasterInvoices.Where(data => data.UserId == userId).ToListAsync();
                if (masterInvoice.Count == 0)
                    return BadRequest("Not Found");

                return Ok(masterInvoice);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("Courses/{userId}")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByUserID(int userId)
        {
            try
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
            catch
            {
                return StatusCode(500, "Unknown error occurred");  
            }
            
        }
    }
}