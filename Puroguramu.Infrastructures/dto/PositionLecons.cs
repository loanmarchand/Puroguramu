using System.ComponentModel.DataAnnotations;

namespace Puroguramu.Infrastructures.dto;

public class PositionLecons
{
    [Key]
    public string IdPositionLecons { get; set; }

    public Lecons Lecons { get; set; }

    public int Position { get; set; }
}
