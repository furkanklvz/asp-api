
namespace WebApplication1.Entity;

public class Product
{
    public int Id { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
}