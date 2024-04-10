using System.ComponentModel.DataAnnotations;

namespace Puroguramu.Infrastructures.dto;

public class Exercices
{
    [Key]
    public string IdExercice { get; set; }

    public string Titre { get; set; }

    public string? Enonce { get; set; }

    public string? Modele { get; set; }

    public string? Solution { get; set; }

    public bool EstVisible { get; set; }

    public string? Difficulte { get; set; }
}
