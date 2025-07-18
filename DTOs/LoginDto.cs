// LoginDto.cs
using System.ComponentModel.DataAnnotations;
namespace OpeningExplorer.DTOs;
/// <summary>
/// Informations pour la connexion.
/// </summary>
public class LoginDto {
    /// <summary>Nom d'utilisateur.</summary>
    [Required] public string Username { get; set; }
    /// <summary>Mot de passe.</summary>
    [Required] public string Password { get; set; }
}