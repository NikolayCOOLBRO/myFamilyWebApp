using Microsoft.EntityFrameworkCore;
using MyFamily.Models;

namespace MyFamily.Database
{
    public interface IMyFamilyDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<FinancialOperation> FinancialOperations { get; set; }

        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
