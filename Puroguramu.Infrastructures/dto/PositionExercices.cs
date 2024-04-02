using System.ComponentModel.DataAnnotations;

namespace Puroguramu.Infrastructures.dto;

public class PositionExercices
{
    [Key]
    public string IdPositionExercices { get; set; }

    public Exercices Exercices { get; set; }

    public int Position { get; set; }
}
