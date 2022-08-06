using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasterInvoiceController : ControllerBase
    {
        private readonly DataContext dataContext;
        public MasterInvoiceController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<MasterInvoice>>> Get()
        {
            return Ok(await this.dataContext.MasterInvoices.ToListAsync());
        }

        [HttpGet("GetByMasterID/{id}")]
        public async Task<ActionResult<MasterInvoice>> GetByMInvoiceID(int id)
        {
            var masterInvoice = await this.dataContext.MasterInvoices.FindAsync(id);
            if (masterInvoice == null)
                return BadRequest("Not Found");
            return Ok(masterInvoice);
        }

        [HttpGet("GetByUID/{userId}")]
        public async Task<ActionResult<MasterInvoice>> GetByUserID(int userId)
        {
            var masterInvoice = await this.dataContext.MasterInvoices.Where(data => data.UserId == userId).ToListAsync();
            if (masterInvoice.Count == 0)
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
        public async Task<ActionResult<List<MasterInvoice>>> Delete(int id)
        {
            var masterInvoice = await this.dataContext.MasterInvoices.FindAsync(id);
            if (masterInvoice == null)
                return BadRequest("Not Found");

            this.dataContext.MasterInvoices.Remove(masterInvoice);
            await this.dataContext.SaveChangesAsync();

            return Ok(await this.dataContext.MasterInvoices.ToListAsync());
        }
    }
}