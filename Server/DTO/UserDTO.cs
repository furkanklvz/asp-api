
namespace Server.DTO;

public class UserDTO
{
    public string UserName { get; set; } = null!;
    public string Email { get; set;} = null!;
    public string? FullName { get; set; }
    public string Token { get; set; } = null!;

}