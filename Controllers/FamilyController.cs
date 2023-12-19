using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFamily.Database;
using MyFamily.Models;

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
        public async Task<ActionResult<Customer>> CreateCustomer(string name, string logIn, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(logIn) || string.IsNullOrWhiteSpace(logIn))
            {
                return BadRequest();
            }

            var newCustomer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = name,
                LogIn = logIn,
                FinancialOperations = new List<FinancialOperation>()
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
        public async Task<ActionResult<Customer>> AddFinancialOperation(Guid idCustomer, string description, string category, int amount, CancellationToken cancellationToken)
        {
            var customer = await _familyDbContext.Customers.FirstOrDefaultAsync(item => item.Id.Equals(idCustomer));

            var financialOperation = new FinancialOperation()
            {
                Id = Guid.NewGuid(),
                Description = description,
                Category = category,
                Amount = amount,
                Date = DateTime.UtcNow,
                CustomerId = customer.Id
            };

            if (customer.FinancialOperations == null)
            {
                customer.FinancialOperations = new List<FinancialOperation>();
            }

            customer.FinancialOperations.Add(financialOperation);
            _familyDbContext.FinancialOperations.Add(financialOperation);
            await _familyDbContext.SaveChangesAsync(cancellationToken);

            return Ok(customer);
        }

        [HttpGet("GetFinancialOperationOnCustomer")]
        public async Task<ActionResult<ICollection<FinancialOperation>>> GetFinancialOperationOnCustomer(Guid idCustomer, CancellationToken cancellationToken)
        {
            var result = await _familyDbContext.Customers.FirstOrDefaultAsync(item => item.Id.Equals(idCustomer));
            return Ok(result.FinancialOperations);
        }
    }
}