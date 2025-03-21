namespace Server.Entity;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string?  FullName { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public string? AddresLine { get; set; }
    public string? CustomerId { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public List<OrderItem> OrderItems { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal GetTotal(){
        return SubTotal + DeliveryFee;
    }
}

public class OrderItem{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string? ProductName { get; set; }
    public string? ProductImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public enum OrderStatus
{
    Pending, PurchaseFailed, Approved, Delivered
}