namespace MyFamily.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LogIn { get; set; }

        public ICollection<FinancialOperation> FinancialOperations { get; set; }
    }
}
