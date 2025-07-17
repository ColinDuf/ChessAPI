// AuthController.cs
using Microsoft.AspNetCore.Mvc;
using OpeningExplorer.DTOs;
using OpeningExplorer.Services;

namespace OpeningExplorer.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IAuthService _svc;
    public AuthController(IAuthService svc) { _svc = svc; }

    [HttpPost("register")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Register(RegisterDto dto) {
        var id = await _svc.Register(dto);
        return CreatedAtAction(nameof(Register), new { id });
    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login(LoginDto dto) {
        try {
            var token = await _svc.Login(dto);
            return Ok(new { token });
        } catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        }
    }
}