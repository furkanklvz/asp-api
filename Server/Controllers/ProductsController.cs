using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Entity;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController (DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            string query = "SELECT TOP (10) * FROM Products";
            var products = await _context.Products.FromSqlRaw(query).ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            string query = "SELECT * FROM Products WHERE ProductId=@id";
            var product = await _context.Products.FromSqlRaw(query, new SqlParameter("@id", id)).FirstOrDefaultAsync();
            if (product == null) return NotFound("The product could not be found.");
            return Ok(product);
        }
    }
}
