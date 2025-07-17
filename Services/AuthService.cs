// AuthService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using OpeningExplorer.Data;
using OpeningExplorer.DTOs;
using OpeningExplorer.Entities;

namespace OpeningExplorer.Services;
public class AuthService : IAuthService {
  private readonly DataContext _ctx;
  private readonly IConfiguration _config;
  public AuthService(DataContext ctx, IConfiguration config) { _ctx = ctx; _config = config; }

  public async Task<int> Register(RegisterDto dto) {
    if (await _ctx.Users.AnyAsync(u => u.Username == dto.Username))
      throw new ArgumentException("Utilisateur existant");
    using var hmac = new HMACSHA512();
    var user = new User {
      Username = dto.Username,
      PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
      PasswordSalt = hmac.Key
    };
    _ctx.Users.Add(user);
    await _ctx.SaveChangesAsync();
    return user.Id;
  }

  public async Task<string> Login(LoginDto dto) {
    var user = await _ctx.Users.SingleOrDefaultAsync(u => u.Username == dto.Username)
               ?? throw new ArgumentException("Utilisateur introuvable");
    using var hmac = new HMACSHA512(user.PasswordSalt);
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
    if (!hash.SequenceEqual(user.PasswordHash)) throw new ArgumentException("Mot de passe incorrect");

    // Génération JWT
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
    var tokenDescriptor = new SecurityTokenDescriptor {
      Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
      Expires = DateTime.UtcNow.AddHours(4),
      Issuer = _config["Jwt:Issuer"],
      Audience = _config["Jwt:Audience"],
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}