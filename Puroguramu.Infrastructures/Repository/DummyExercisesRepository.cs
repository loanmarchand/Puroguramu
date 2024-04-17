using Puroguramu.Domains;
using Puroguramu.Domains.Repository;

namespace Puroguramu.Infrastructures.Repository;

public class DummyExercisesRepository : IExercisesRepository
{
    public int GetExercisesCount() => throw new NotImplementedException();

    public Exercise GetExercise(string exerciseId)
        => new Exercise();

    public Task CreateExerciceAsync(string leconTitre, string inputTitre) => throw new NotImplementedException();
}
