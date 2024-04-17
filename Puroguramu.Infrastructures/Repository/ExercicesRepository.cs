using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;
using Puroguramu.Infrastructures.Mapper;

namespace Puroguramu.Infrastructures.Repository;

public class ExercicesRepository : IExercisesRepository
{
    private readonly PurogumaruContext _context;

    public ExercicesRepository(PurogumaruContext context) => _context = context;

    public int GetExercisesCount() => _context.Exercices.Count();

    public Exercise GetExercise(string exerciseId) => DtoMapper.MapExercices(_context.Exercices.Find(exerciseId));

    public Task CreateExerciceAsync(string leconTitre, string inputTitre)
    {
        var lecon = _context.Lecons
            .Include(e => e.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            throw new Exception("Lecon not found");
        }

        var exercice = new Exercices { IdExercice = Guid.NewGuid().ToString(), Titre = inputTitre, EstVisible = true };
        lecon.ExercicesList.Add(exercice);
        _context.SaveChanges();
        return Task.CompletedTask;
    }
}
