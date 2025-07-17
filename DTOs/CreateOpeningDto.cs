// CreateOpeningDto.cs
using System.ComponentModel.DataAnnotations;
namespace OpeningExplorer.DTOs;
public class CreateOpeningDto {
    [Required] public string ECO { get; set; }
    [Required] public string Name { get; set; }
    public string Moves { get; set; }
    [Range(0,100)] public double WinRate { get; set; }
    public int Frequency { get; set; }
}