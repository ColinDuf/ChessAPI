// Opening.cs
using System.ComponentModel.DataAnnotations;
namespace OpeningExplorer.Entities;
public class Opening {
    public int Id { get; set; }
    [Required] public string ECO { get; set; }
    [Required] public string Name { get; set; }
    public string Moves { get; set; }  // ex: "e4 e5 Nf3 Nc6"
    [Range(0, 100)] public double WinRate { get; set; }
    public int Frequency { get; set; }
}