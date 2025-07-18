// CreateOpeningDto.cs
using System.ComponentModel.DataAnnotations;
namespace OpeningExplorer.DTOs;
/// <summary>
/// Données pour créer ou modifier une ouverture.
/// </summary>
public class CreateOpeningDto {
    /// <summary>Code ECO de l'ouverture.</summary>
    [Required] public string ECO { get; set; }
    /// <summary>Nom de l'ouverture.</summary>
    [Required] public string Name { get; set; }
    /// <summary>Succession de coups.</summary>
    public string Moves { get; set; }
    /// <summary>Pourcentage de victoires pour les Blancs.</summary>
    [Range(0,100)] public double WinRate { get; set; }
    /// <summary>Popularité de l'ouverture.</summary>
    public int Frequency { get; set; }
}