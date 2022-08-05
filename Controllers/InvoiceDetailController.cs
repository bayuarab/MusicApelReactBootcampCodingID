using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
  [ApiController]
    [Route("api/[controller]")]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly DataContext dataContext;

        public InvoiceDetailController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet("{invoiceId}")]
        public async Task<ActionResult<InvoiceDetail>> Get(string invoiceId)
        {
            var z = invoiceId;
            var data = await this.dataContext.InvoiceDetails.
            Join(this.dataContext.Courses,
                ind => ind.FKCourse,
                c => c.Id,
                (ind, c) => new {ind, c}).
            Join(this.dataContext.CourseCategories,
                indc => indc.c.FKCategory,
                cc => cc.Id,
                (indccc, cc) => new {indccc, cc}).
            Join(this.dataContext.MasterInvoices,
                indccm => indccm.indccc.ind.NoInvoice,
                mi => mi.NoInvoice,
                (indccm, mi) => new {indccm, mi}).
            Select(result => new { 
                NoInvoice = result.indccm.indccc.ind.NoInvoice,
                Course = result.indccm.indccc.c.CourseTitle,
                Category = result.indccm.cc.Category,
                Schedule = result.indccm.indccc.c.Jadwal,
                Price = result.indccm.indccc.c.Price,
                Cost = result.mi.Cost,
                PurchasedTime = result.mi.PurchaseDate
            }).
            Where(result => result.NoInvoice == invoiceId).ToListAsync();

            if (data.Count == 0)
                return BadRequest("Not Found");

            return Ok(data);  
        }

        [HttpPost]
        public async Task<ActionResult<List<InvoiceDetail>>> AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            this.dataContext.InvoiceDetails.Add(invoiceDetail);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<MasterInvoice>>> Delete(int id)
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