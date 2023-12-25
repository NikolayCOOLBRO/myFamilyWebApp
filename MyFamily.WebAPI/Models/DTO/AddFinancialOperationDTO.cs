namespace MyFamily.Models.DTO
{
    public class AddFinancialOperationDTO
    {
        public Guid IdCustomer { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedDate { get; set;}
    }
}
