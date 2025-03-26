namespace Server.DTO;

public class CreateOrderDTO
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? City { get; set; }
    public string? AddresLine { get; set; }
    public string CardHolderName { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
    public string CardExpireMonth { get; set; } = null!;
    public string CardExpireYear { get; set; } = null!;
    public string CardCvc { get; set; } = null!;
}
