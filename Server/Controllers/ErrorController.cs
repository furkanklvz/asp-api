using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Controller;
[Route("/api/[controller]")]
[ApiController]
public class ErrorController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    [HttpGet("not-found")]
    public IActionResult GetNotFoundError() //404
    {
        return NotFound();
    }
    [HttpGet("bad-request")]
    public IActionResult GetBadRequestError() //400
    {
        return BadRequest();
    }
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorizedErrror() //401
    {
        return Unauthorized();
    }
    [HttpGet("valid-error")]
    public IActionResult GetValidationError() //400
    {
        ModelState.AddModelError("Valid error 1", "error details 1");
        ModelState.AddModelError("Valid error 2", "error details 2");
        return ValidationProblem();
    }

    [HttpGet("server-error")]
    public IActionResult GetServerError() //500
    {
        throw new Exception("server error");
    }
}