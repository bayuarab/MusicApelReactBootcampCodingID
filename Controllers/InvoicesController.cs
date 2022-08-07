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
            return Ok(await this.dataContext.MasterInvoices.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MasterInvoice>> GetMasterInvoiceById(int id)
        {
            var masterInvoice = await this.dataContext.MasterInvoices.FindAsync(id);
            if (masterInvoice == null)
                return BadRequest("Not Found");
            return Ok(masterInvoice);
        }

        [HttpGet("NoInvoice/{noInvoice}")]
        public async Task<ActionResult<MasterInvoice>> GetMasterInvoiceByNumber(string noInvoice)
        {
            var masterInvoice = await this.dataContext.MasterInvoices.FirstOrDefaultAsync(result => result.NoInvoice == noInvoice);
            if (masterInvoice == null)
                return BadRequest("Not Found");
            return Ok(masterInvoice);
        }

        [HttpPost]
        public async Task<ActionResult<List<MasterInvoice>>> AddMasterInvoice(MasterInvoice masterInvoice)
        {
            this.dataContext.MasterInvoices.Add(masterInvoice);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.MasterInvoices.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<MasterInvoice>>> UpdateMasterInvoice(MasterInvoice request)
        {
            var dbMasterInvoice = await this.dataContext.MasterInvoices.FindAsync(request.Id);
            if(dbMasterInvoice == null)
                return BadRequest("Hero not found");

            dbMasterInvoice.NoInvoice = request.NoInvoice;
            dbMasterInvoice.PurchaseDate = request.PurchaseDate;
            dbMasterInvoice.Qty = request.Qty;
            dbMasterInvoice.Cost = request.Cost;

            await this.dataContext.SaveChangesAsync();
            return Ok(await this.dataContext.MasterInvoices.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<MasterInvoice>>> DeleteMasterInvoice(int id)
        {
            var masterInvoice = await this.dataContext.MasterInvoices.FindAsync(id);
            if (masterInvoice == null)
                return BadRequest("Not Found");

            this.dataContext.MasterInvoices.Remove(masterInvoice);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.MasterInvoices.ToListAsync());
        }


        [HttpGet("Details")]
        public async Task<ActionResult<List<InvoiceDetail>>> GetInvoiceDetails()
        {
            return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
        }

        [HttpGet("DetailsById/{id}")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByID(int id)
        {
            var data = await this.dataContext.InvoiceDetails.
            Join(this.dataContext.Courses,
                ind => ind.CourseId,
                c => c.Id,
                (ind, c) => new {ind, c}).
            Join(this.dataContext.CourseCategories,
                indc => indc.c.CourseCategoryId,
                cc => cc.Id,
                (indccc, cc) => new {indccc, cc}).
            Join(this.dataContext.MasterInvoices,
                indccm => indccm.indccc.ind.NoInvoice,
                mi => mi.NoInvoice,
                (indccm, mi) => new {indccm, mi}).
            Where(result => result.indccm.indccc.ind.Id == id).
            Select(result => new { 
                NoInvoice = result.indccm.indccc.ind.NoInvoice,
                Course = result.indccm.indccc.c.CourseTitle,
                Category = result.indccm.cc.Category,
                Schedule = result.indccm.indccc.c.Jadwal,
                Price = result.indccm.indccc.c.Price,
                Cost = result.mi.Cost,
                PurchasedTime = result.mi.PurchaseDate
            }).ToListAsync();

            if (data.Count == 0)
                return BadRequest("Not Found");

            return Ok(data);  
        }

        [HttpGet("DetailsByNoInvoice/{noInvoice}")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetailByNoInvoice(string noInvoice)
        {
            var invoiceDetails = await this.dataContext.MasterInvoices.
            Join(this.dataContext.InvoiceDetails,
                mi => mi.Id,
                ind => ind.MasterInvoiceId,
                (mi, ind) => new {mi, ind}).
            Join(this.dataContext.Courses,
                mind => mind.ind.CourseId,
                c => c.Id,
                (mindc, c) => new {mindc, c}).
            Join(this.dataContext.CourseCategories,
                mindc => mindc.c.CourseCategoryId,
                cc => cc.Id,
                (mindccc, cc) => new {mindccc, cc}).
            Where(result => result.mindccc.mindc.mi.NoInvoice == noInvoice).
            Select(result => new {
                NoInvoice = result.mindccc.mindc.ind.NoInvoice,
                Course = result.mindccc.c.CourseTitle,
                Category = result.cc.Category,
                Schedule = result.mindccc.c.Jadwal,
                Cost = result.mindccc.mindc.mi.Cost,
                PurchasedDate = result.mindccc.mindc.mi.PurchaseDate
            }).ToListAsync();
            
            if (invoiceDetails.Count == 0)
                return BadRequest("Not Found");
            return Ok(invoiceDetails);
        }

        [HttpPost("Details")]
        public async Task<ActionResult<List<InvoiceDetail>>> AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            this.dataContext.InvoiceDetails.Add(invoiceDetail);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
        }

        [HttpPut("Details")]
        public async Task<ActionResult<List<InvoiceDetail>>> UpdateInvoiceDetail(InvoiceDetail request)
        {
            var dbInvoiceDetail = await this.dataContext.InvoiceDetails.FindAsync(request.Id);
            if(dbInvoiceDetail == null)
                return BadRequest("Hero not found");

            dbInvoiceDetail.NoInvoice = request.NoInvoice;
            dbInvoiceDetail.CourseId = request.CourseId;
            dbInvoiceDetail.MasterInvoiceId = request.MasterInvoiceId;

            await this.dataContext.SaveChangesAsync();
            return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
        }
        
        [HttpDelete("Details/{id}")]
        public async Task<ActionResult<List<MasterInvoice>>> DeleteInvoiceDetail(int id)
        {
            var detailInvoice = await this.dataContext.InvoiceDetails.FindAsync(id);
            if ( detailInvoice == null)
                return BadRequest("Not Found");

            this.dataContext.InvoiceDetails.Remove(detailInvoice);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
        }
    }
}