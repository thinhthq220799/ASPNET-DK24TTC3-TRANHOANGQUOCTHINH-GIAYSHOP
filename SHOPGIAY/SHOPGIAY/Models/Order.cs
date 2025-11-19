namespace SHOPGIAY.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? ShippingAddress { get; set; }

        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal TotalAmount { get; set; }

        public string? Note { get; set; }

        // Navigation
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
