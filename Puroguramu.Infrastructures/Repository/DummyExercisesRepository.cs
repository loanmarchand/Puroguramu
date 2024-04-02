using Puroguramu.Domains;
using Puroguramu.Domains.Repository;

namespace Puroguramu.Infrastructures.Repository;

public class DummyExercisesRepository : IExercisesRepository
{
    public int GetExercisesCount() => throw new NotImplementedException();

    public Exercise GetExercise(Guid exerciseId)
        => new Exercise();
}
