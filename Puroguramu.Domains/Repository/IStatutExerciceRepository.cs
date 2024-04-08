namespace Puroguramu.Domains.Repository;

public interface IStatutExerciceRepository
{
    Status? GetStatut(string getExerciceId, string getUserId);
    Task CreateStatut(string getExerciceId, string getUserId);
    Task UpdateStatut(string getExerciceId, string getUserId, ExerciseStatus resultStatus);
}
