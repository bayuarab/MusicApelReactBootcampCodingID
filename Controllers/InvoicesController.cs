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

        [HttpGet]
        public async Task<ActionResult<List<MasterInvoice>>> GetMasterInvoice()
        {
            try
            {
                return Ok(await this.dataContext.MasterInvoices.ToListAsync());
            }
            catch 
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("AllInvoices")]
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
                        purchasedDate = result.mi.PurchaseDate
                    }).ToListAsync();

                

                return Ok(data);
            }
            catch 
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }      

        [HttpGet("{id}")]
        public async Task<ActionResult<MasterInvoice>> GetMasterInvoiceById(int id)
        {
            try
            {
                var masterInvoice = await this.dataContext.MasterInvoices.FindAsync(id);
                if (masterInvoice == null)
                    return BadRequest("Not Found");
                return Ok(masterInvoice);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("NoInvoice/{noInvoice}")]
        public async Task<ActionResult<MasterInvoice>> GetMasterInvoiceByNumber(string noInvoice)
        {
            try
            {
                var masterInvoice = await this.dataContext.MasterInvoices.FirstOrDefaultAsync(result => result.NoInvoice == noInvoice);
                if (masterInvoice == null)
                    return BadRequest("Not Found");
                return Ok(masterInvoice);
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost]
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

                return Ok(await this.dataContext.MasterInvoices.ToListAsync());
            }
            catch 
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        // [HttpPut]
        // public async Task<ActionResult<List<MasterInvoice>>> UpdateMasterInvoice(MasterInvoice request)
        // {
        //     try
        //     {
        //         var dbMasterInvoice = await this.dataContext.MasterInvoices.FindAsync(request.Id);
        //         if(dbMasterInvoice == null)
        //             return BadRequest("Hero not found"); 

        //         dbMasterInvoice.NoInvoice = request.NoInvoice;
        //         dbMasterInvoice.PurchaseDate = request.PurchaseDate;
        //         dbMasterInvoice.Qty = request.Qty;
        //         dbMasterInvoice.Cost = request.Cost;

        //         await this.dataContext.SaveChangesAsync();
        //         return Ok(await this.dataContext.MasterInvoices.ToListAsync());
        //     }
        //     catch 
        //     {
        //         return StatusCode(500, "Unknown error occurred");
        //     }
        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<List<MasterInvoice>>> DeleteMasterInvoice(int id)
        // {
        //     try
        //     {
        //         var masterInvoice = await this.dataContext.MasterInvoices.FindAsync(id);
        //         if (masterInvoice == null)
        //             return BadRequest("Not Found");

        //         this.dataContext.MasterInvoices.Remove(masterInvoice);
        //         await this.dataContext.SaveChangesAsync();

        //         return Ok(await this.dataContext.MasterInvoices.ToListAsync());
        //     }
        //     catch 
        //     {
        //         return StatusCode(500, "Unknown error occurred");
        //     }
        // }


        [HttpGet("Details")]
        public async Task<ActionResult<List<InvoiceDetail>>> GetInvoiceDetails()
        {
            try
            {
                return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpGet("DetailsById/{id}")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByID(int id)
        {
            try
            {
                var data = await this.dataContext.InvoiceDetails.
                Join(this.dataContext.MasterInvoices,
                    ind=> ind.MasterInvoiceId,
                    mi => mi.Id,
                    (ind, mi) => new {ind, mi}).
                Where(result => result.ind.Id == id).
                Select(result => new { 
                    NoInvoice = result.ind.NoInvoice,
                    Course = result.ind.Course,
                    Category = result.ind.CourseCategory,
                    Schedule = result.ind.Schedule,
                    Price = result.ind.Price,
                    Cost = result.mi.Cost,
                    PurchasedTime = result.mi.PurchaseDate
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

        [HttpGet("DetailsByMasterId/{masterId}")]
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
        


        [HttpGet("DetailsByNoInvoice/{noInvoice}")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByNoInvoice(string noInvoice)
        {
            try
            {
                var data = await this.dataContext.InvoiceDetails.
                Join(this.dataContext.MasterInvoices,
                    ind=> ind.MasterInvoiceId,
                    mi => mi.Id,
                    (ind, mi) => new {ind, mi}).
                Where(result => result.ind.NoInvoice == noInvoice).
                Select(result => new { 
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

        [HttpPost("Details")]
        public async Task<ActionResult<List<InvoiceDetail>>> AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            try
            {
                var validMasterId = await this.dataContext.MasterInvoices.FindAsync(invoiceDetail.MasterInvoiceId);
                var validId = await this.dataContext.InvoiceDetails.FindAsync(invoiceDetail.Id);
                var validCourseId = await this.dataContext.Courses.FirstOrDefaultAsync(data => data.CourseTitle == invoiceDetail.Course);
                if((validMasterId == null) || (validId != null) || (validCourseId == null))
                    return BadRequest("Not valid data");

                if(validMasterId.NoInvoice != invoiceDetail.NoInvoice)
                    return BadRequest("Not valid data");

                this.dataContext.InvoiceDetails.Add(invoiceDetail);
                await this.dataContext.SaveChangesAsync();
                
                return Ok(await this.dataContext.InvoiceDetails.ToListAsync());    
            }
            catch
            {
                 return StatusCode(500, "Unknown error occurred");
            }
        }

        // [HttpPut("Details")]
        // public async Task<ActionResult<List<InvoiceDetail>>> UpdateInvoiceDetail(InvoiceDetail request)
        // {
        //     try
        //     {
        //         var dbInvoiceDetail = await this.dataContext.InvoiceDetails.FindAsync(request.Id);
        //         if (dbInvoiceDetail == null)
        //             return BadRequest("Not valid data");

        //         var validNoInvoice = await this.dataContext.MasterInvoices.FirstOrDefaultAsync(data => data.NoInvoice == request.NoInvoice);
        //         if (validNoInvoice == null)
        //             return BadRequest("Not valid data");

        //         var validCourseId = await this.dataContext.Courses.FirstOrDefaultAsync(data => data.CourseTitle == request.Course);
        //         if (validCourseId == null)
        //             return BadRequest("Not valid data");

        //         var validMasterId = await this.dataContext.MasterInvoices.FindAsync(request.MasterInvoiceId);
        //         if (validMasterId == null)
        //             return BadRequest("Not valid data");

        //         dbInvoiceDetail.NoInvoice = request.NoInvoice;
        //         dbInvoiceDetail.Course = request.Course;
        //         dbInvoiceDetail.CourseCategory = request.CourseCategory;
        //         dbInvoiceDetail.Price
        //         dbInvoiceDetail.MasterInvoiceId = request.MasterInvoiceId;

        //         await this.dataContext.SaveChangesAsync();
        //         return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
        //     }
        //     catch 
        //     {
        //         return StatusCode(500, "Unknown error occurred");
        //     }
        // }
        
        // [HttpDelete("Details/{id}")]
        // public async Task<ActionResult<List<MasterInvoice>>> DeleteInvoiceDetail(int id)
        // {
        //     try
        //     {
        //         var detailInvoice = await this.dataContext.InvoiceDetails.FindAsync(id);
        //         if ( detailInvoice == null)
        //             return BadRequest("Not Found");

        //         this.dataContext.InvoiceDetails.Remove(detailInvoice);
        //         await this.dataContext.SaveChangesAsync();

        //         return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
        //     }
        //     catch 
        //     {
        //         return StatusCode(500, "Unknown error occurred");
        //     }
        // }
    }
}