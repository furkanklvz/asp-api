namespace Server.DTO;

public class CartDTO
{
    public int CartId { get; set; }
    public string? CustomerId { get; set; }
    public List<CartItemDTO> CartItems { get; set; } = new();
}

public class CartItemDTO
{
    public int ProductId { get; set; }
    public string? productName { get; set; }
    public decimal? ProductPrice { get; set; }
    public string? ProductImageUrl { get; set; }
    public int Quantity { get; set; }
}