// OpeningDto.cs
namespace OpeningExplorer.DTOs;
/// <summary>
/// Représente une ouverture pour l'affichage.
/// </summary>
public class OpeningDto {
    /// <summary>Identifiant interne.</summary>
    public int Id { get; set; }
    /// <summary>Code ECO.</summary>
    public string ECO { get; set; }
    /// <summary>Nom de l'ouverture.</summary>
    public string Name { get; set; }
    /// <summary>Liste des coups.</summary>
    public string Moves { get; set; }
    /// <summary>Taux de victoire en pourcentage.</summary>
    public double WinRate { get; set; }
    /// <summary>Fréquence d'apparition.</summary>
    public int Frequency { get; set; }
}