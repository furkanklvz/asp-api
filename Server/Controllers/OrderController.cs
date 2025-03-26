using System.Security.Claims;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
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
public class OrderController(DataContext context, IConfiguration config) : ControllerBase
{
    private readonly DataContext _context = context;
    private readonly IConfiguration _config = config;

    [HttpGet]
    public async Task<ActionResult<PagedDataDTO<OrderDTO>>> GetOrders(int pageSize, int firstItemIndex = 0)
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (customerId == null) return BadRequest("User not found.");

        var query = _context.Orders
            .AsQueryable();

        var totalRecords = await query.CountAsync();

        var orders = await query
            .Where(i => i.CustomerId == customerId)
            .OrderByDescending(i => i.OrderDate)
            .Skip(firstItemIndex)
            .Take(pageSize)
            .Include(i => i.OrderItems)
            .Select(i => OrderToDTO(i))
            .ToListAsync();
        
        var pagedData = new PagedDataDTO<OrderDTO>{
            Data = orders,
            TotalRecords = totalRecords,
            LastItemIndex = firstItemIndex + pageSize - 1,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
        return Ok(pagedData);
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
            Name = orderRequest.Name,
            Surname = orderRequest.Surname,
            Phone = orderRequest.Phone,
            SubTotal = subTotal,
            OrderItems = cart.CartItems.Select(i => i.Product == null ? null : new Entity.OrderItem
            {
                Price = i.Product.Price,
                Quantity = i.Quantity,
                ProductName = i.Product.Name,
                ProductImageUrl = i.Product.ImageUrl,
                ProductId = i.ProductId
            }
            ).OfType<Entity.OrderItem>().ToList()
        };

        var paymentResult = await ProcessPayment(cart, orderRequest);
        if (paymentResult.Status == "failure") return BadRequest(paymentResult.ErrorMessage);

        order.ConversationId = paymentResult.ConversationId;
        order.BasketId = paymentResult.BasketId;

        _context.Orders.Add(order);
        _context.Carts.Remove(cart);

        var success = await _context.SaveChangesAsync() > 0;
        if (success) return CreatedAtAction(nameof(GetOrder), new { orderId = order.OrderId }, OrderToDTO(order));
        return BadRequest(new ProblemDetails { Title = "The order could not be created.", Status = 400 });
    }

    private async Task<Payment> ProcessPayment(Cart cart, CreateOrderDTO orderRequest)
    {
        Options options = new Options();
        options.ApiKey = _config["PaymentAPI:APIKey"];
        options.SecretKey = _config["PaymentAPI:SecretKey"];
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = Guid.NewGuid().ToString();
        request.Price = cart.CalculateTotalCost().ToString();
        request.PaidPrice = (cart.CalculateTotalCost() * 1.2).ToString();
        request.Currency = Currency.TRY.ToString();
        request.Installment = 1;
        request.BasketId = cart.CartId.ToString();
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        PaymentCard paymentCard = new PaymentCard();
        paymentCard.CardHolderName = orderRequest.CardHolderName;
        paymentCard.CardNumber = orderRequest.CardNumber;
        paymentCard.ExpireMonth = orderRequest.CardExpireMonth;
        paymentCard.ExpireYear = orderRequest.CardExpireYear;
        paymentCard.Cvc = orderRequest.CardCvc;
        paymentCard.RegisterCard = 0;
        request.PaymentCard = paymentCard;

        Buyer buyer = new Buyer();
        buyer.Id = "BY789";
        buyer.Name = orderRequest.Name;
        buyer.Surname = orderRequest.Surname;
        buyer.GsmNumber = orderRequest.Phone;
        buyer.Email = orderRequest.Email;
        buyer.IdentityNumber = User.FindFirstValue(ClaimTypes.NameIdentifier);
        buyer.LastLoginDate = "2015-10-05 12:43:35";
        buyer.RegistrationDate = "2013-04-21 15:12:09";
        buyer.RegistrationAddress = orderRequest.AddresLine;
        buyer.Ip = "85.34.78.112";
        buyer.City = orderRequest.City;
        buyer.Country = "Turkey";
        buyer.ZipCode = "34732";
        request.Buyer = buyer;

        Address shippingAddress = new Address();
        shippingAddress.ContactName = orderRequest.Name + " " + orderRequest.Surname;
        shippingAddress.City = orderRequest.City;
        shippingAddress.Country = "Turkey";
        shippingAddress.Description = orderRequest.AddresLine;
        shippingAddress.ZipCode = "34742";
        request.ShippingAddress = shippingAddress;

        request.BillingAddress = shippingAddress;

        List<BasketItem> basketItems = new List<BasketItem>();


        foreach (var item in cart.CartItems)
        {
            BasketItem basketItem = new BasketItem();
            basketItem.Id = item.CartItemId.ToString();
            basketItem.Name = item.Product.Name;
            basketItem.Category1 = "testCategory";
            basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            basketItem.Price = ((double)item.Product.Price * item.Quantity).ToString();
            basketItems.Add(basketItem);
        }

        request.BasketItems = basketItems;

        return await Payment.Create(request, options);
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
            Name = order.Name,
            Surname = order.Surname,
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