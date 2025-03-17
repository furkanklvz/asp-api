namespace Server.Entity;

public class Cart
{
    public int CartId { get; set; }
    public string? CustomerId { get; set; }
    public List<CartItem> CartItems { get; set; } = new();

    public void AddCartItem(Product product, int quantity){
        var cartItem = CartItems.FirstOrDefault(item => item.ProductId == product.ProductId);

        if (cartItem == null){
            CartItems.Add(new CartItem{
                ProductId = product.ProductId,
                Product = product,
                Quantity = quantity,
                CartId = CartId
            });
        }else
        {
            cartItem.Quantity += quantity;
        }
    }

    public void DeleteCartItem(int productId, int quantity){
        var cartItem = CartItems.FirstOrDefault(item => item.ProductId == productId);
        if(cartItem == null) return;
        cartItem.Quantity -= quantity;
        if (cartItem.Quantity <= 0)
        {
            CartItems.Remove(cartItem);
        }
    }

}

public class CartItem
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int CartId { get; set; }
    public int Quantity { get; set; }

}