// RegisterDto.cs
using System.ComponentModel.DataAnnotations;
namespace OpeningExplorer.DTOs;
/// <summary>
/// Données nécessaires pour l'inscription d'un utilisateur.
/// </summary>
public class RegisterDto {
    /// <summary>Nom d'utilisateur choisi.</summary>
    [Required] public string Username { get; set; }
    /// <summary>Mot de passe.</summary>
    [Required] public string Password { get; set; }
}