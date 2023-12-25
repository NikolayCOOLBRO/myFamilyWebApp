using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFamily.Database;
using MyFamily.Models;
using MyFamily.Models.DTO;

namespace MyFamily.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMyFamilyDbContext _familyDbContext;

        public CustomersController(IMyFamilyDbContext dbContext)
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
    }
}