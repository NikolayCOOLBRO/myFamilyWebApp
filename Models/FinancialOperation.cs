namespace MyFamily.Models
{
    public class FinancialOperation
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
    }
}