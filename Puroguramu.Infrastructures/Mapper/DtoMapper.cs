using Puroguramu.Domains;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Mapper;

public class DtoMapper
{
    public static Lecon MapLecon(Lecons lecons)
    {
        return new Lecon
        {
            Titre = lecons.Titre,
            Description = lecons.Description,
            estVisible = lecons.estVisible,
            ExercicesList = lecons.ExercicesList.Select(MapExercices).ToList(),
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
        };
    }
}
