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
        private readonly ILogger<FamilyController> _logger;
        private readonly IMyFamilyDbContext _familyDbContext;

        public FamilyController(ILogger<FamilyController> logger, IMyFamilyDbContext dbContext)
        {
            _logger = logger;
            _familyDbContext = dbContext;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<Customer>> CreateCustomer(string name, string logIn, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(logIn) || string.IsNullOrWhiteSpace(logIn))
            {
                return BadRequest();
            }

            var newUser = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = name,
                LogIn = logIn,
                FinancialOperations = new List<FinancialOperation>()
            };

            _familyDbContext.Customers.Add(newUser);
            await _familyDbContext.SaveChangesAsync(cancellationToken);

            return Ok(newUser);
        }

        [HttpGet("GetUserLogIn")]
        public async Task<ActionResult<Customer>> GetCustomerLogIn(string logIn)
        {
            if (string.IsNullOrEmpty(logIn) || string.IsNullOrWhiteSpace(logIn))
            {
                return BadRequest();
            }

            var user = await _familyDbContext.Customers.FirstOrDefaultAsync(item => item.LogIn.Equals(logIn));

            return Ok(user);
        }
    }
}