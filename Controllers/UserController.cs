using Microsoft.AspNetCore.Mvc;
// "ConnectionStrings": {
//   "DefaultConnection": "server=localhost\\sqlexpress;database=SecondVDb;trusted_connection=true"
// },
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

        public class InvoiceDetails
        {
            public string? NoInvoice { get; set; }
            public int CourseId { get; set; }
            public int ScheduleId { get; set; }
            public int MasterInvoiceId { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUser()
        {
            try
            {
                return Ok(await this.dataContext.Users.ToListAsync());
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("AllUser")]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            try
            {
                var data = await this.dataContext.Users.
                Where(data => data.roles != "admin").
                Select(result => new
                {
                    result.nama,
                    result.email
                }).ToListAsync();

                return Ok(data);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUsers(User user)
        {
            try
            {
                var validEmail = await this.dataContext.Users.FirstOrDefaultAsync(data => data.email == user.email);
                if (validEmail != null)
                    return BadRequest("Email sudah terdaftar");

                this.dataContext.Users.Add(user);
                await this.dataContext.SaveChangesAsync();

                return Ok("Registrasi sukses");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpDelete("{userEmail}")]
        public async Task<ActionResult<User>> DeleteUser(string userEmail)
        {
            try
            {
                var user = await this.dataContext.Users.FirstOrDefaultAsync(user => user.email == userEmail);
                if (user == null)
                    return NotFound("Not Found");

                this.dataContext.Users.Remove(user);
                await this.dataContext.SaveChangesAsync();

                return Ok("Success");
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        // [HttpPost("Login")]
        // public async Task<ActionResult<List<User>>> UserLogin(User request)
        // {
        //     try
        //     {
        //         var userValid = await this.dataContext.Users.FirstOrDefaultAsync(result => result.email == request.email);
        //         if (userValid == null)
        //             return BadRequest("Akun tidak ditemukan");

        //         if (userValid.password != request.password)
        //             return BadRequest("Password Salah");

        //         var valid = await this.dataContext.Users.
        //         Where(result => result.email == request.email).
        //         Select(result => new
        //         {
        //             id = result.Id,
        //             roles = result.roles,
        //             nama = result.nama
        //         }).ToListAsync();

        //         return Ok(valid[0]);
        //     }
        //     catch (System.Exception)
        //     {
        //         return StatusCode(500, "Unknown error occurred");
        //     }
        // }

        // [HttpPut("ChangePassword")]
        // public async Task<ActionResult<User>> ChangeUserPass(User request)
        // {
        //     Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
        //     try
        //     {
        //         var validUser = await this.dataContext.Users.FindAsync(request.Id);
        //         if (validUser == null)
        //             return BadRequest("Not valid data");

        //         if (validUser.email != request.email)
        //             return BadRequest("Not valid data");
                
        //         // validUser.password = request.password;

        //         await this.dataContext.SaveChangesAsync();

        //         await dbContextTransaction.CommitAsync();

        //         return Ok("Success");
        //     }
        //     catch (System.Exception)
        //     {
        //         await dbContextTransaction.RollbackAsync();
        //         return StatusCode(500, "Unknown error occurred");
        //     }            
        // }

        // [HttpPost("PasswordValidation")]
        // public async Task<ActionResult<List<User>>> ValidationChangePassword(User request)
        // {
        //     try
        //     {
        //         var validUser = await this.dataContext.Users.FindAsync(request.Id);
        //         if (validUser == null)
        //             return BadRequest("Not valid data");

        //         if (validUser.email != request.email)
        //             return BadRequest("Not valid data");
                
        //         // if (validUser.password != request.password)
        //             // return BadRequest("Data not valid");

        //         return Ok("Success");
        //     }
        //     catch (System.Exception)
        //     {
        //         return StatusCode(500, "Unknown error occurred");
        //     }            
        // }

        [HttpPut("ChangeName")]
        public async Task<ActionResult<User>> ChangeUserName(User request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validUser = await this.dataContext.Users.FindAsync(request.Id);
                if (validUser == null)
                    return BadRequest("Not valid data");

                if (validUser.email != request.email)
                    return BadRequest("Not valid data");

                validUser.nama = request.nama;

                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return Ok("Success");
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost("CheckEmail")]
        public async Task<ActionResult<List<User>>> CheckEmail(User request)
        {
            try
            {
                var checkEmail = await this.dataContext.Users.FirstOrDefaultAsync(result => result.email == request.email);
                if (checkEmail == null)
                {
                    return BadRequest("Email tidak ditemukan");
                }

                return Ok("Succes");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost("MInvoice")]
        public async Task<ActionResult<List<MasterInvoice>>> AddMasterInvoice(MasterInvoice masterInvoice)
        {
            try
            {
                var validId = await this.dataContext.MasterInvoices.FindAsync(masterInvoice.Id);
                // var validNoInvoice = await this.dataContext.MasterInvoices.FirstOrDefaultAsync(data => data.NoInvoice == masterInvoice.NoInvoice);
                var validUserId = await this.dataContext.Users.FindAsync(masterInvoice.UserId);
                if (validId != null || validUserId == null)
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
        public async Task<ActionResult<List<InvoiceDetail>>> AddInvoiceDetail(InvoiceDetails request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validMasterId = await this.dataContext.MasterInvoices.FindAsync(request.MasterInvoiceId);
                if (validMasterId == null)
                    return BadRequest("Not valid data");

                var validCourse = await this.dataContext.Courses.
                Join(this.dataContext.CourseCategories,
                    c => c.CourseCategoryId,
                    cc => cc.Id,
                    (c, cc) => new { c, cc }).
                Select(result => new
                {
                    result.c.Id,
                    result.c.CourseTitle,
                    result.cc.Category,
                    result.c.Price
                }).FirstOrDefaultAsync(data => data.Id == request.CourseId);

                if (validCourse == null)
                    return BadRequest("Not valid data");

                var validSchedule = await this.dataContext.Schedules.FindAsync(request.ScheduleId);
                if (validSchedule == null)
                    return BadRequest("Not valid data");

                if (validMasterId.NoInvoice != request.NoInvoice)
                    return BadRequest("Not valid data");


                this.dataContext.InvoiceDetails.Add(entity: new InvoiceDetail
                {
                    NoInvoice = request.NoInvoice,
                    CourseCategory = validCourse.Category,
                    Course = validCourse.CourseTitle,
                    Schedule = validSchedule.jadwal,
                    Price = validCourse.Price,
                    MasterInvoiceId = request.MasterInvoiceId
                });

                this.dataContext.UserCourses.Add(entity: new UserCourse
                {
                    CourseId = request.CourseId,
                    ScheduleId = request.ScheduleId,
                    UserId = validMasterId.UserId,
                });

                await this.dataContext.SaveChangesAsync();
                // await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return Ok(await this.dataContext.InvoiceDetails.Where(result => result.NoInvoice == request.NoInvoice).ToListAsync());
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        // [HttpPost("Course")]
        // public async Task<ActionResult<List<Course>>> AddUserCourses(UserCourse request)
        // {
        //     try
        //     {
        //         var validUser = this.dataContext.Users.FindAsync(request.UserId);
        //         if (validUser == null)
        //             return BadRequest("Not valid data");

        //         var validCourse = this.dataContext.Courses.FindAsync(request.UserId);
        //         if (validUser == null)
        //             return BadRequest("Not valid data");
        //     }
        //     catch (System.Exception)
        //     {

        //         throw;
        //     }
        // }

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
                    (cac, cc) => new { cac, cc }).
                Join(this.dataContext.Schedules,
                    cacc => cacc.cac.ca.ScheduleId,
                    s => s.id,
                    (caccs, s) => new { caccs, s }).
                Where(data => data.caccs.cac.ca.UserId == userID).
                Select(result => new
                {
                    CourseId = result.caccs.cac.c.Id,
                    Course = result.caccs.cac.c.CourseTitle,
                    CourseImage = result.caccs.cac.c.CourseImage,
                    Category = result.caccs.cc.Category,
                    Schedule = result.s.jadwal,
                    ScheduleId = result.s.id,
                    Price = result.caccs.cac.c.Price,
                    Id = result.caccs.cac.ca.Id
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

        [HttpDelete("Cart/ByCourseId/{UserId}/{courseId}")]
        public async Task<ActionResult<List<Cart>>> DeleteByCourseId(int UserId, int courseId)
        {
            try
            {
                var userCart = await this.dataContext.Carts.FirstOrDefaultAsync(data => data.UserId == UserId && data.CourseId == courseId);
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

        [HttpGet("InvoicesDetails/{UserId}/{noInvoice}")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByNoInvoice(int UserId, string noInvoice)
        {
            try
            {
                var data = await this.dataContext.InvoiceDetails.
                Join(this.dataContext.MasterInvoices,
                    ind => ind.MasterInvoiceId,
                    mi => mi.Id,
                    (ind, mi) => new { ind, mi }).
                Where(result => result.ind.NoInvoice == noInvoice && result.mi.UserId == UserId).
                Select(result => new
                {
                    NoInvoice = result.ind.NoInvoice,
                    Course = result.ind.Course,
                    Category = result.ind.CourseCategory,
                    Schedule = result.ind.Schedule,
                    Price = result.ind.Price,
                    Cost = result.mi.Cost,
                    purchasedDate = result.mi.PurchaseDate
                }).ToListAsync();

                if (data.Count == 0)
                    return BadRequest("Not Found");

                return Ok(data);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("Courses/{userId}")]
        public async Task<ActionResult<UserCourse>> GetCourseByUserID(int userId)
        {
            try
            {
                var courseData = await this.dataContext.UserCourses.
                Join(this.dataContext.Courses,
                    uc => uc.CourseId,
                    c => c.Id,
                    (uc, c) => new { uc, c }).
                Join(this.dataContext.CourseCategories,
                    ucc => ucc.c.CourseCategoryId,
                    cc => cc.Id,
                    (ucc, cc) => new { ucc, cc }).
                Join(this.dataContext.Schedules,
                    ucccc => ucccc.ucc.uc.ScheduleId,
                    s => s.id,
                    (ucccc, s) => new { ucccc, s }).
                Where(data => data.ucccc.ucc.uc.UserId == userId).
                Select(result => new
                {
                    CourseImage = result.ucccc.ucc.c.CourseImage,
                    CourseId = result.ucccc.ucc.c.Id,
                    Course = result.ucccc.ucc.c.CourseTitle,
                    Category = result.ucccc.cc.Category,
                    Schedule = result.s.jadwal
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

        // [HttpGet("Courses/{userId}")]
        // public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByUserID(int userId)
        // {
        //     try
        //     {
        //         var courseData = await this.dataContext.MasterInvoices.
        //         Join(this.dataContext.InvoiceDetails,
        //             mi => mi.Id,
        //             ind => ind.MasterInvoiceId,
        //             (mi, ind) => new { mi, ind }).
        //         Join(this.dataContext.Courses,
        //             mind => mind.ind.Course,
        //             c => c.CourseTitle,
        //             (mindc, c) => new { mindc, c }).
        //         Join(this.dataContext.CourseCategories,
        //             mindc => mindc.c.CourseCategoryId,
        //             cc => cc.Id,
        //             (mindccc, cc) => new { mindccc, cc }).
        //         Where(result => result.mindccc.mindc.mi.UserId == userId).
        //         Select(result => new
        //         {
        //             CourseImage = result.mindccc.c.CourseImage,
        //             CourseId = result.mindccc.c.Id,
        //             Course = result.mindccc.c.CourseTitle,
        //             Category = result.cc.Category,
        //             Schedule = result.mindccc.mindc.ind.Schedule
        //         }).ToListAsync();

        //         if (courseData.Count == 0)
        //             return BadRequest("Not Found");

        //         return Ok(courseData);
        //     }
        //     catch
        //     {
        //         return StatusCode(500, "Unknown error occurred");
        //     }
        // }
    }
}