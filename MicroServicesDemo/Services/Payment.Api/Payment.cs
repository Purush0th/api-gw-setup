namespace Payment.Api
{
    public class Payment
    {
        public DateOnly Date { get; set; }
        public int Amount { get; set; }
        public Guid TransactionId { get; set; } = Guid.NewGuid();
        public string? Status { get; set; }
    }
}
