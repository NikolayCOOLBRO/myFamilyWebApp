using Microsoft.AspNetCore.Mvc;
using MyFamily.Database;
using MyFamily.Models.DTO;
using MyFamily.Models;
using Microsoft.EntityFrameworkCore;

namespace MyFamily.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialController : ControllerBase
    {
        private readonly IMyFamilyDbContext _familyDbContext;

        public FinancialController(IMyFamilyDbContext dbContext)
        {
            _familyDbContext = dbContext;
        }

        [HttpPost("AddFinancialOperation")]
        public async Task<ActionResult<FinancialOperation>> AddFinancialOperation([FromBody] AddFinancialOperationDTO addFinancialOperationDTO, CancellationToken cancellationToken)
        {
            if (_familyDbContext.Customers.FirstOrDefault(item => item.Id.Equals(addFinancialOperationDTO.IdCustomer)) == null)
            {
                return BadRequest();
            }

            var financialOperation = new FinancialOperation()
            {
                Id = Guid.NewGuid(),
                Description = addFinancialOperationDTO.Description,
                Category = addFinancialOperationDTO.Category,
                Amount = addFinancialOperationDTO.Amount,
                Date = addFinancialOperationDTO.CreatedDate,
                CustomerId = addFinancialOperationDTO.IdCustomer
            };

            _familyDbContext.FinancialOperations.Add(financialOperation);
            await _familyDbContext.SaveChangesAsync(cancellationToken);

            return Ok(financialOperation);
        }

        [HttpGet("GetFinancialOperationOnCustomerById")]
        public async Task<ActionResult<ICollection<FinancialOperation>>> GetFinancialOperationOnCustomerById(Guid idCustomer, CancellationToken cancellationToken)
        {
            return Ok(await FindFinancialOperationByCustomerId(idCustomer, cancellationToken));
        }

        private async Task<List<FinancialOperation>> FindFinancialOperationByCustomerId(Guid idCustomer, CancellationToken cancellationToken)
        {
            var result = new List<FinancialOperation>();

            await _familyDbContext.FinancialOperations.ForEachAsync(item =>
            {
                if (item.CustomerId.Equals(idCustomer))
                {
                    result.Add(item);
                }
            }, cancellationToken);

            return result;
        }
    }
}
