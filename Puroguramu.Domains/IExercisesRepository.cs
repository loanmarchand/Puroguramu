namespace Puroguramu.Domains;

public interface IExercisesRepository
{
    Exercise GetExercise(Guid exerciseId);
}