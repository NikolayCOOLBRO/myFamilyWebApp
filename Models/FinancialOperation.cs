namespace MyFamily.Models
{
    public class FinancialOperation
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}