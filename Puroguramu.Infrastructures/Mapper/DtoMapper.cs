using Puroguramu.Domains;
using Puroguramu.Infrastructures.dto;
using Status = Puroguramu.Domains.Status;

namespace Puroguramu.Infrastructures.Mapper;

public class DtoMapper
{
    public static Lecon MapLecon(Lecons lecons, int? exoFait, int? exoTotal)
    {
        return new Lecon
        {
            Titre = lecons.Titre,
            Description = lecons.Description,
            estVisible = lecons.estVisible,
            ExercicesFait = exoFait ?? 0,
            ExercicesTotal = exoTotal ?? 0,
            ExercicesList = lecons.ExercicesList.Select(MapExercices).ToList(),
        };
    }

    public static Cour MapCours(Cours cours)
    {
        return new Cour
        {
            Titre = cours.Titre,
            ImageUrl = cours.ImageUrl,

            // Assurez-vous que cours.Lecons n'est pas null avant de l'utiliser
            Lecons = cours.Lecons.Select(l => MapLecon(l, null, null)).ToList(),
        };
    }

    public static Exercise MapExercices(Exercices exercices)
    {
        return new Exercise
        {
            Titre = exercices.Titre,
            Enonce = exercices.Enonce,
            Modele = exercices.Modele,
            Solution = exercices.Solution,
            EstVisible = exercices.EstVisible,
            Difficulte = exercices.Difficulte,
            etat = Status.NotStarted,
        };
    }

    public static Lecon MapLeconWithStatuts(Lecons lecons, IEnumerable<(Exercices exercice, dto.Status? statut)> exercicesStatuts)
    {
        var exercices = exercicesStatuts.Select(es => MapExercicesWithStatut(es.exercice, es.statut)).ToList();

        return new Lecon
        {
            Titre = lecons.Titre,
            Description = lecons.Description,
            estVisible = lecons.estVisible,
            ExercicesList = exercices,

            // Ces champs peuvent nécessiter des ajustements selon les besoins spécifiques de votre application
            ExercicesFait = exercices.Count(e => e.etat == Status.Passed),
            ExercicesTotal = exercices.Count,
        };
    }

    public static Exercise MapExercicesWithStatut(Exercices exercices, dto.Status? statut)
    {
        return new Exercise
        {
            Titre = exercices.Titre,
            Enonce = exercices.Enonce,
            Modele = exercices.Modele,
            Solution = exercices.Solution,
            EstVisible = exercices.EstVisible,
            Difficulte = exercices.Difficulte,
            etat = MapStatus(statut ?? dto.Status.NotStarted), // Assurez-vous d'utiliser une valeur par défaut appropriée
        };
    }

    private static Status MapStatus(dto.Status statut)
    {
        switch (statut)
        {
            case dto.Status.Failed:
                return Status.Failed;
            case dto.Status.NotStarted:
                return Status.NotStarted;
            case dto.Status.Started:
                return Status.Started;
            case dto.Status.Passed:
                return Status.Passed;
            default:
                return Status.NotStarted; // Valeur par défaut si le statut n'est pas reconnu
        }
    }
}
