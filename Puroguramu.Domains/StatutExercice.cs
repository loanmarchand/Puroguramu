namespace Puroguramu.Domains;

public class StatutExercice
{
    public Exercise Exercice { get; set; }

    public Utilisateur Etudiant { get; set; }

    public Status Statut { get; set; }
    public string SolutionTempo { get; set; }
}
