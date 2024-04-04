using Puroguramu.Domains;
using Puroguramu.Infrastructures.dto;
using Status = Puroguramu.Domains.Status;

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
            // Assurez-vous que lecons.ExercicesList n'est pas null avant de l'utiliser
            ExercicesList = lecons.ExercicesList?.Select(MapExercices).ToList() ?? new List<Exercise>(),
        };
    }

    public static Cour MapCours(Cours cours)
    {
        return new Cour
        {
            Titre = cours.Titre,
            ImageUrl = cours.ImageUrl,
            // Assurez-vous que cours.Lecons n'est pas null avant de l'utiliser
            Lecons = cours.Lecons?.Select(MapLecon).ToList() ?? new List<Lecon>(),
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


}
