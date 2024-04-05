using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.Mapper;
using Status = Puroguramu.Infrastructures.dto.Status;

namespace Puroguramu.Infrastructures.Repository;

public class CoursRepository : ICoursRepository
{
    private readonly PurogumaruContext _context;

    public CoursRepository(PurogumaruContext context)
    {
        _context = context;
    }

    public IList<Cour> GetCours()
    {
        var coursList = _context.Cours
            .Include(c => c.Lecons)
            .ThenInclude(l => l.ExercicesList)
            .ToList();

        return coursList.Select(DtoMapper.MapCours).ToList();
    }

    public IEnumerable<Lecon> GetLeconsForCours(string nameCours, string userId)
    {
        var lecona = new List<Lecon>();
        var lecons = _context.Cours
            .Include(c => c.Lecons)
            .ThenInclude(l => l.ExercicesList)
            .FirstOrDefault(c => c.Titre == nameCours)
            ?.Lecons;

        foreach (var lecon in lecons)
        {
            var nombreExercices = lecon.ExercicesList.Count;
            var nombreExercicesFait = _context.StatutExercices
                .Count(se => lecon.ExercicesList.Select(e => e.IdExercice).Contains(se.Exercice.IdExercice) &&
                             se.Etudiant.Id == userId &&
                             se.Statut == Status.Passed);

            lecona.Add(DtoMapper.MapLecon(lecon, nombreExercicesFait, nombreExercices));
        }

        return lecona;
    }
}

