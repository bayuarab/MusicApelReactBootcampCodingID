using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecondV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private DataContext dataContext;

        public PaymentController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        
        [HttpGet, Authorize(Roles = "admin")]
        public async Task<ActionResult<List<PaymentMethod>>> GetPaymentMethod()
        {
            try
            {
                var data = await this.dataContext.PaymentMethods.ToListAsync();
                return Ok(data);    
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<PaymentMethod>> AddPaymentMethod(PaymentMethod request)
        {
            try
            {
                var validMethod = await this.dataContext.PaymentMethods.FirstOrDefaultAsync(data => data.Method == request.Method);
                if (validMethod != null)
                    return BadRequest("Not valid data");
                
                this.dataContext.PaymentMethods.Add(request);
                await this.dataContext.SaveChangesAsync();

                return Ok("Success");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }            
        }

        [HttpPut, Authorize(Roles = "admin")]
        public async Task<ActionResult<PaymentMethod>> EditPaymentMethod(PaymentMethod request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validMethod = await this.dataContext.PaymentMethods.FindAsync(request.Id);
                if (validMethod == null)
                    return BadRequest("Not valid data");
                
                validMethod.Method = request.Method;
                validMethod.Icon = request.Icon;

                await this.dataContext.SaveChangesAsync();

                var validTransaction = await this.dataContext.PaymentMethods.Where(data => data.Method == request.Method).ToListAsync();
                if (validTransaction.Count > 1) {
                    await dbContextTransaction.RollbackAsync();    
                    return BadRequest("Not valid data, rollback");
                }

                await dbContextTransaction.CommitAsync();

                return Ok("Success");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }            
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<PaymentMethod>> DeleteMethod(int id)
        {
            try
            {
                var validMethod = await this.dataContext.PaymentMethods.FindAsync(id);
                if (validMethod == null)
                    return NotFound("Not Found");

                this.dataContext.PaymentMethods.Remove(validMethod);
                await this.dataContext.SaveChangesAsync();
 
                return Ok("Success");
            }
            catch
            {
                return StatusCode(500, "Unknown error occurred");
            }
        }

    }
}