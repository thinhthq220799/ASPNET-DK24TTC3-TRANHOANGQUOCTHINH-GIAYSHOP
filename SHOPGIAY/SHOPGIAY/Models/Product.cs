namespace SHOPGIAY.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public string? ProductName { get; set; }
        public string? Sku { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // Navigation
        public Category? Category { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
