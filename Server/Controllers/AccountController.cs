using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Entity;
using Server.Services;

namespace Server.Controller;

[Route("api/[controller]")]
[ApiController]
public class AccountController(UserManager<AppUser> userManager, TokenService tokenService) : ControllerBase
{

    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly TokenService _tokenSerive = tokenService;

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null) return BadRequest("User could not found.");

        var result = await _userManager.CheckPasswordAsync(user, model.Password);

        if (result) return Ok(new UserDTO
        {
            Email = user.Email!,
            FullName = user.FullName,
            Token = await _tokenSerive.GenerateJWT(user),
            UserName = user.UserName!,
        });
        return Unauthorized("User name or password is wrong.");
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
    {

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = new AppUser
        {
            UserName = model.UserName,
            Email = model.Email,
            FullName = model.FullName,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Customer");
            return CreatedAtAction(nameof(Login), new UserDTO
            {
                Email = user.Email!,
                FullName = user.FullName,
                Token = await _tokenSerive.GenerateJWT(user),
                UserName = user.UserName!,
            });
        }
        return BadRequest(result.Errors.First().Description);
    }


}