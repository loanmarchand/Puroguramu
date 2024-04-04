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
        var leconsLists = new List<Lecon>();
        var lecons = _context.Cours
            .Include(l => l.Lecons)
            .ThenInclude(l => l.ExercicesList)
            .Where(c => c.Titre == nameCours);

        foreach (var lecon in lecons)
        {
            var leconDto = DtoMapper.MapLecon(lecon.Lecons.FirstOrDefault()!);

            leconDto.ExercicesFait = 0;//TODO: Get the number of exercises done by the user
            leconDto.ExercicesTotal = 0;//TODO: Get the total number of exercises

            leconsLists.Add(leconDto);
        }

        return leconsLists;
    }
}
