using Puroguramu.Domains;

namespace Puroguramu.Infrastructures.Dummies;

public class DummyExercisesRepository : IExercisesRepository
{
    public Exercise GetExercise(Guid exerciseId)
        => new Exercise();
}