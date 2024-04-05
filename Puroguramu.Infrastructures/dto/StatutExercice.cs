using System.ComponentModel.DataAnnotations;

namespace Puroguramu.Infrastructures.dto;

public class StatutExercice
{
    [Key]
    public string IdStatutExercice { get; set; }

    public Exercices Exercice { get; set; }

    public Utilisateurs Etudiant { get; set; }

    public Status Statut { get; set; }

    public string? SolutionTempo { get; set; }
}
