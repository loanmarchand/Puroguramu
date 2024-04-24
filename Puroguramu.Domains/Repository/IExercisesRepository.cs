namespace Puroguramu.Domains.Repository;

public interface IExercisesRepository
{
    int GetExercisesCount();

    Exercise GetExercise(string exerciseId);

    Task<bool> CreateExerciceAsync(string leconTitre, string inputTitre);
    Task<bool> DeleteExercice(string leconTitre, string exerciceTitre);
    Task<bool> ChangeVisibility(string leconTitre, string exerciceTitre);
    Task<bool> MoveExercice(string leconTitre, string exerciceTitre, string direction);
    Exercise GetExercise(string leconTitre, string exerciceTitre);
    Task<bool> UpdateExercise(Exercise exercice, string tempTitre, string leconTitre);
}
