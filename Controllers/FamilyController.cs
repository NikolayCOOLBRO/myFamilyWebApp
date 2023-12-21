using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFamily.Database;
using MyFamily.Models;
using MyFamily.Models.DTO;

namespace MyFamily.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyController : ControllerBase
    {
        private readonly IMyFamilyDbContext _familyDbContext;

        public FamilyController(IMyFamilyDbContext dbContext)
        {
            _familyDbContext = dbContext;
        }

        [HttpPost("CreateCustomer")]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CreateCustomerDTO createCustomer, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(createCustomer.Name) || string.IsNullOrWhiteSpace(createCustomer.Name) || string.IsNullOrEmpty(createCustomer.LogIn) || string.IsNullOrWhiteSpace(createCustomer.LogIn))
            {
                return BadRequest();
            }

            var newCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = createCustomer.Name,
                LogIn = createCustomer.LogIn
            };

            _familyDbContext.Customers.Add(newCustomer);
            await _familyDbContext.SaveChangesAsync(cancellationToken);

            return Ok(newCustomer);
        }

        [HttpGet("GetCustomerLogIn")]
        public async Task<ActionResult<Customer>> GetCustomerLogIn(string logIn, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(logIn) || string.IsNullOrWhiteSpace(logIn))
            {
                return BadRequest();
            }

            var customer = await _familyDbContext.Customers.FirstOrDefaultAsync(item => item.LogIn.Equals(logIn), cancellationToken);

            return Ok(customer);
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

        [HttpGet("GetFinancialOperationOnCustomerId")]
        public async Task<ActionResult<ICollection<FinancialOperation>>> GetFinancialOperationOnCustomerId(Guid idCustomer, CancellationToken cancellationToken)
        {
            var result = new List<FinancialOperation>();

            await _familyDbContext.FinancialOperations.ForEachAsync(item =>
            {
                if (item.CustomerId.Equals(idCustomer))
                {
                    result.Add(item);
                }
            }, cancellationToken);


            return Ok(result);
        }

        [HttpGet("GetFinancialOperationOnCustomerLogIn")]
        public async Task<ActionResult<ICollection<FinancialOperation>>> GetFinancialOperationOnCustomerLogIn(string login, CancellationToken cancellationToken)
        {
            var customer = await _familyDbContext.Customers.FirstOrDefaultAsync(item => item.LogIn.Equals(login), cancellationToken);

            var result = new List<FinancialOperation>();

            await _familyDbContext.FinancialOperations.ForEachAsync(item =>
            {
                if (item.CustomerId.Equals(customer.Id))
                {
                    result.Add(item);
                }
            }, cancellationToken);

            return Ok(result);
        }
    }
}