namespace Puroguramu.Domains.Repository;

public interface IStatutExerciceRepository
{
    IEnumerable<StatutExercice> GetStatutExercicesForCoursAndEtudiant(string coursTitre, string etudiantId);
}
