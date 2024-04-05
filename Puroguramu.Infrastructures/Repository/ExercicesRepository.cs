using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.Mapper;

namespace Puroguramu.Infrastructures.Repository;

public class ExercicesRepository : IExercisesRepository
{
    private readonly PurogumaruContext _context;

    public ExercicesRepository(PurogumaruContext context) => _context = context;

    public int GetExercisesCount() => _context.Exercices.Count();

    public Exercise GetExercise(string exerciseId)
    {
        return DtoMapper.MapExercices(_context.Exercices.Find(exerciseId));
    }
}
