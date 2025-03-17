using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTO;
using Server.Entity;

namespace Server.Controller;

[Route("api/[controller]")]
[ApiController]
public class CartController(DataContext dataContext) : ControllerBase
{

    private readonly DataContext _context = dataContext;

    [HttpGet]
    public async Task<ActionResult<CartDTO>> GetCart(string customerId)
    {
        var cart = await GetCartOrCreate(customerId);
        return Ok(CartToDTO(cart));
    }

    [HttpPost]
    public async Task<ActionResult<CartDTO>> AddCartItem(string customerId, int productId,int quantity){
        var cart = await GetCartOrCreate(customerId);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        if(product == null) return NotFound("The product could not be founded.");
        cart.AddCartItem(product, quantity);
        var success = await _context.SaveChangesAsync() > 0;
        if (success) return CreatedAtAction(nameof(GetCart),CartToDTO(cart));
        return BadRequest("The item could not be added.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCartItem(string customerId, int productId,int quantity){
        var cart = await GetCartOrCreate(customerId);
        cart.DeleteCartItem(productId, quantity);
        var success = await _context.SaveChangesAsync() > 0;
        if(success) return Ok();
        return BadRequest("The item could not be deleted.");
    }

    private async Task<Cart> GetCartOrCreate(string customerId)
    {
        var cart = await _context.Carts
        .Include(i => i.CartItems)
        .ThenInclude(i => i.Product)
        .Where(i => i.CustomerId == customerId)
        .FirstOrDefaultAsync();

        if (cart == null)
        {
            cart = new Cart {CustomerId = customerId};
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }
        return cart;
    }

    private CartDTO CartToDTO(Cart cart){
        return new CartDTO{
            CustomerId = cart.CustomerId,
            CartId = cart.CartId,
            CartItems = cart.CartItems.Select(i => 
                new CartItemDTO{
                    ProductId = i.ProductId,
                    ProductImageUrl = i.Product?.ImageUrl,
                    productName = i.Product?.Name,
                    ProductPrice = i.Product?.Price,
                    Quantity = i.Quantity
                }
            ).ToList()
        };
    }
}