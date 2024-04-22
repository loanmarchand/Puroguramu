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
            Description = lecons.Description!,
            EstVisible = lecons.estVisible,
            ExercicesFait = exoFait ?? 0,
            ExercicesTotal = exoTotal ?? 0,
            ExercicesList = lecons.ExercicesList!.Select(MapExercices).ToList()
        };
    }

    public static Exercise MapExercices(Exercices exercices) =>
        new()
        {
            Titre = exercices.Titre,
            Enonce = exercices.Enonce!,
            Modele = exercices.Modele!,
            Solution = exercices.Solution!,
            EstVisible = exercices.EstVisible,
            Difficulte = exercices.Difficulte!,
            Etat = Status.NotStarted
        };

    public static Lecon? MapLeconWithStatuts(Lecons lecons, IEnumerable<(Exercices exercice, dto.Status? statut)> exercicesStatuts)
    {
        var exercices = exercicesStatuts.Select(es => MapExercicesWithStatut(es.exercice, es.statut)).ToList();

        return new Lecon
        {
            Titre = lecons.Titre,
            Description = lecons.Description,
            EstVisible = lecons.estVisible,
            ExercicesList = exercices,

            // Ces champs peuvent nécessiter des ajustements selon les besoins spécifiques de votre application
            ExercicesFait = exercices.Count(e => e.Etat == Status.Passed),
            ExercicesTotal = exercices.Count,
        };
    }

    public static Exercise MapExercicesWithStatut(Exercices exercices, dto.Status? statut) =>
        new()
        {
            Titre = exercices.Titre,
            Enonce = exercices.Enonce!,
            Modele = exercices.Modele!,
            Solution = exercices.Solution!,
            EstVisible = exercices.EstVisible,
            Difficulte = exercices.Difficulte!,
            Etat = MapStatus(statut ?? dto.Status.NotStarted) // Assurez-vous d'utiliser une valeur par défaut appropriée
        };

    private static Status MapStatus(dto.Status statut) =>
        statut switch
        {
            dto.Status.Failed => Status.Failed,
            dto.Status.NotStarted => Status.NotStarted,
            dto.Status.Started => Status.Started,
            dto.Status.Passed => Status.Passed,
            _ => Status.NotStarted
        };
}
