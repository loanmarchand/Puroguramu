using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;

namespace Puroguramu.Infrastructures.Repository;

public class ExercicesRepository : IExercisesRepository
{
    private readonly PurogumaruContext _context;

    public ExercicesRepository(PurogumaruContext context) => _context = context;

    public int GetExercisesCount() => _context.Exercices.Count();

    public Exercise GetExercise(Guid exerciseId) => throw new NotImplementedException();
}
