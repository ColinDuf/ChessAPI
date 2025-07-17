// OpeningDto.cs
namespace OpeningExplorer.DTOs;
public class OpeningDto {
    public int Id { get; set; }
    public string ECO { get; set; }
    public string Name { get; set; }
    public string Moves { get; set; }
    public double WinRate { get; set; }
    public int Frequency { get; set; }
}