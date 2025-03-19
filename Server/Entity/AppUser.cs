using Microsoft.AspNetCore.Identity;

namespace Server.Entity;

public class AppUser: IdentityUser
{
    public string? FullName { get; set; }
}