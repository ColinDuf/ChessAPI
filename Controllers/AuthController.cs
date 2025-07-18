// AuthController.cs
using Microsoft.AspNetCore.Mvc;
using OpeningExplorer.DTOs;
using OpeningExplorer.Services;

namespace OpeningExplorer.Controllers;
/// <summary>
/// Endpoints d'authentification des utilisateurs.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IAuthService _svc;
    public AuthController(IAuthService svc) { _svc = svc; }

    /// <summary>
    /// Inscrit un nouvel utilisateur.
    /// </summary>
    /// <param name="dto">Informations de création du compte.</param>
    /// <returns>Identifiant du nouvel utilisateur.</returns>
    [HttpPost("register")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Register(RegisterDto dto) {
        var id = await _svc.Register(dto);
        return CreatedAtAction(nameof(Register), new { id });
    }

    /// <summary>
    /// Authentifie un utilisateur et renvoie un jeton JWT.
    /// </summary>
    /// <param name="dto">Identifiants de connexion.</param>
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