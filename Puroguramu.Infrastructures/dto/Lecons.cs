using System.ComponentModel.DataAnnotations;

namespace Puroguramu.Infrastructures.dto;

public class Lecons
{
    [Key]
    public string IdLecons { get; set; }

    public string Titre { get; set; }

    public string? Description { get; set; }

    public bool estVisible { get; set; }

    public IList<Exercices>? ExercicesList { get; set; }
}
