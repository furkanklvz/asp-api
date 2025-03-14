using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Entity;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductsController(DataContext context) : ControllerBase
{

    private readonly DataContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int? id)
    {
        if (id != null)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
        else
        {
            return BadRequest();
        }

    }
}