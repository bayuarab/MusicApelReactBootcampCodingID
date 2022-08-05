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
            var detailInvoice = await this.dataContext.InvoiceDetails.Where(x => x.NoInvoice == invoiceId).ToListAsync();
            
            if (detailInvoice.Count == 0)
                return BadRequest("Not Found");

            return Ok(detailInvoice);
        }

        [HttpPost]
        public async Task<ActionResult<List<InvoiceDetail>>> AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            this.dataContext.InvoiceDetails.Add(invoiceDetail);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.InvoiceDetails.ToListAsync());
        }       
    }
}