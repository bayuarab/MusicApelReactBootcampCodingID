using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
  [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private DataContext dataContext;

        public InvoicesController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet("AllInvoices"), Authorize(Roles = "admin")]
        public async Task<ActionResult<List<MasterInvoice>>> GetAllInvoices()
        {
            try
            {
                var data = await this.dataContext.MasterInvoices.
                    Join(this.dataContext.Users,
                        mi => mi.UserId,
                        us => us.Id,
                        (mi, us) => new{mi, us}).
                    Where(data => data.us.roles != "admin").
                    Select(result => new {
                        result.us.nama,
                        masterInvoiceId = result.mi.Id,
                        result.mi.NoInvoice,
                        purchasedDate = result.mi.PurchaseDate,
                        result.mi.Method
                    }).ToListAsync();

                return Ok(data);
            }
            catch 
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }      

        [HttpGet("DetailsByMasterId/{masterId}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByMasterId(int masterId)
        {
            try
            {
                var data = await this.dataContext.InvoiceDetails.
                Join(this.dataContext.MasterInvoices,
                    ind=> ind.MasterInvoiceId,
                    mi => mi.Id,
                    (ind, mi) => new {ind, mi}).
                Where(result => result.mi.Id == masterId).
                Select(result => new { 
                    NoInvoice = result.ind.NoInvoice,
                    Course = result.ind.Course,
                    Category = result.ind.CourseCategory,
                    Schedule = result.ind.Schedule,
                    Price = result.ind.Price,
                    Cost = result.mi.Cost,
                    PurchasedDate = result.mi.PurchaseDate
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
    }
}