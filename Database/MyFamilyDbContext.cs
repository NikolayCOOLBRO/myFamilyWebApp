using Microsoft.EntityFrameworkCore;
using MyFamily.Models;

namespace MyFamily.Database
{
    public class MyFamilyDbContext : DbContext, IMyFamilyDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FinancialOperation> FinancialOperations { get; set; }

        public MyFamilyDbContext(DbContextOptions<MyFamilyDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
