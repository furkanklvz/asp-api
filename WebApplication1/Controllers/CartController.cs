using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Entity;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CartController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet]
    public async Task<ActionResult<CartDTO>> GetCart()
    {
        var cart = await GetOrCreate();
        return CartToDTO(cart);
    }

    [HttpPost]
    public async Task<ActionResult> AddItemToCart(int productId, int quantity)
    {
        var cart = await GetOrCreate();

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null) return NotFound("The product is not found.");
        cart.AddItem(product, quantity);

        var success = await _context.SaveChangesAsync() > 0;

        if (success) return CreatedAtAction(nameof(GetCart), CartToDTO(cart));
        return BadRequest(new ProblemDetails { Title = "The product could not be added." });
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCartItem(int productId, int quantity)
    {
        var cart = await GetOrCreate();
        cart.DeleteItem(productId, quantity);
        var success = await _context.SaveChangesAsync() > 0;
        if (success) return NoContent();
        return BadRequest(new ProblemDetails { Title = "The product could not be deleted." });
    }

    private async Task<Cart> GetOrCreate()
    {
        var cart = await _context.Carts
           .Include(i => i.CartItems)
           .ThenInclude(i => i.Product)
           .Where(i => i.CustomerId == Request.Cookies["customerId"])
           .FirstOrDefaultAsync();
        if (cart == null)
        {
            var customerId = Guid.NewGuid().ToString();

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(1),
                IsEssential = true
            };

            Response.Cookies.Append("customerId", customerId, cookieOptions);
            cart = new Cart { CustomerId = customerId };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }
        return cart;
    }

    private CartDTO CartToDTO(Cart cart)
    {
        return new CartDTO
        {
            CartId = cart.CartId,
            CustomerId = cart.CustomerId,
            CartItems = cart.CartItems.Select(item =>
                new CartItemDTO
                {
                    ProductId = item.ProductId,
                    ProductImageUrl = item.Product.ImageUrl,
                    ProductName = item.Product.Name,
                    ProductPrice = item.Product.Price,
                    Quantity = item.Quantity
                }
            ).ToList(),
        };
    }
}