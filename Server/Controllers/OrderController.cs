using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTO;
using Server.Entity;

namespace Server.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<OrderDTO>>> GetOrders()
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (customerId == null) return BadRequest("User not found.");
        var order = await _context.Orders.FromSqlRaw(
            "SELECT * FROM Orders WHERE CustomerId = @customerId", new SqlParameter("@customerId", customerId)
            ).Include(i => i.OrderItems)
            .Select(i => OrderToDTO(i))
            .ToListAsync();
        return Ok(order);
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int orderId)
    {
        var order = await _context.Orders.FromSqlRaw(
            "SELECT * FROM Orders WHERE OrderId = @orderId", new SqlParameter("@orderId", orderId)
            ).Include(i => i.OrderItems)
            .FirstOrDefaultAsync();

        if (order == null) return NotFound("Order not found.");
        return Ok(OrderToDTO(order));
    }

    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderDTO orderRequest)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (customerId == null) return NotFound(new ProblemDetails { Title = "User not found", Status = 404 });
        var deliveryFee = 49.90m;
        var cart = await _context.Carts.FromSqlRaw(
            "SELECT * FROM Carts WHERE CustomerId = @customerId", new SqlParameter("@customerId", customerId)
        ).Include(i => i.CartItems)
        .ThenInclude(i => i.Product)
        .FirstOrDefaultAsync();

        if (cart == null) return BadRequest(new ProblemDetails { Title = "Shopping cart not found.", Status = 400 });
        if (cart.CartItems.Count == 0) return BadRequest(
            new ProblemDetails { Title = "There is not item in the cart.", Status = 400 });
        var subTotal = cart.CartItems.Sum(i => i.Product!.Price * i.Quantity);
        var order = new Order
        {
            CustomerId = customerId,
            DeliveryFee = deliveryFee,
            AddresLine = orderRequest.AddresLine,
            City = orderRequest.City,
            FullName = orderRequest.FullName,
            Phone = orderRequest.Phone,
            SubTotal = subTotal,
            OrderItems = cart.CartItems.Select(i => i.Product == null ? null : new OrderItem
            {
                Price = i.Product.Price,
                Quantity = i.Quantity,
                ProductName = i.Product.Name,
                ProductImageUrl = i.Product.ImageUrl,
                ProductId = i.ProductId
            }
            ).OfType<OrderItem>().ToList()
        };
        _context.Orders.Add(order);
        _context.Carts.Remove(cart);
        var success = await _context.SaveChangesAsync() > 0;
        if (success) return CreatedAtAction(nameof(GetOrder), new {orderId= order.OrderId}, OrderToDTO(order));
        return BadRequest(new ProblemDetails { Title = "The order could not be created.", Status = 400 });
    }

    private static OrderDTO OrderToDTO(Order order)
    {
        return new OrderDTO
        {
            OrderId = order.OrderId,
            AddresLine = order.AddresLine,
            City = order.City,
            CustomerId = order.CustomerId,
            DeliveryFee = order.DeliveryFee,
            FullName = order.FullName,
            OrderStatus = order.OrderStatus,
            OrderDate = order.OrderDate,
            Phone = order.Phone,
            SubTotal = order.SubTotal,
            OrderItems = order.OrderItems.Select(item =>
                new OrderItemDTO
                {
                    OrderItemId = item.OrderItemId,
                    OrderId = item.OrderId,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductImageUrl = item.ProductImageUrl,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                }
            ).ToList()
        };
    }

}