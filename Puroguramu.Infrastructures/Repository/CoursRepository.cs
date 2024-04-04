using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.Mapper;

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
        var lecons = _context.Cours
            .Include(l => l.Lecons)
            .ThenInclude(l => l.ExercicesList)
            .FirstOrDefault(c => c.Titre == nameCours);

        return lecons?.Lecons.Select(DtoMapper.MapLecon).Where(l => l.estVisible).ToList() ?? new List<Lecon>();
    }
}
