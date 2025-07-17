// LoginDto.cs
using System.ComponentModel.DataAnnotations;
namespace OpeningExplorer.DTOs;
public class LoginDto {
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
}