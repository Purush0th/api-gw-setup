namespace Order.Api
{
    public class OrderDto
    {
        public DateOnly Date { get; set; }

        public int Id { get; set; }

        public IEnumerable<OrderItem> Items { get; set; }

        public decimal Total
        {
            get
            {
                return this.Items.Sum(i => i.Price);
            }
        }

        public Guid ShippingId { get; set; }
        public Guid TransactionId { get; set; }
    }
}
