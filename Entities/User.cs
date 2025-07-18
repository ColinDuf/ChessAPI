// User.cs
using System.ComponentModel.DataAnnotations;

namespace OpeningExplorer.Entities;
/// <summary>
/// Utilisateur de l'application.
/// </summary>
public class User {
    public int Id { get; set; }
    [Required] public string Username { get; set; }
    [Required] public byte[] PasswordHash { get; set; }
    [Required] public byte[] PasswordSalt { get; set; }
}