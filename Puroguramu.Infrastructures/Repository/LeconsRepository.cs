using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.Mapper;

namespace Puroguramu.Infrastructures.Repository;

public class LeconsRepository : ILeconsRepository
{
    private readonly PurogumaruContext _context;

    public LeconsRepository(PurogumaruContext context) => _context = context;

    public IList<Lecon> GetLecons()
    {
        var lecons = _context.Lecons
            .Include(l => l.ExercicesList);
        return  lecons.Select(DtoMapper.MapLecon).ToList();
    }

    public Lecon GetLecon(string idLecons)
    {
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == idLecons);
        return DtoMapper.MapLecon(lecon);
    }

    public void AddLecon(Lecon lecon) => throw new NotImplementedException();

    public void UpdateLecon(Lecon lecon) => throw new NotImplementedException();

    public void DeleteLecon(Lecon lecon) => throw new NotImplementedException();

    public IEnumerable<Exercise>? GetExercicesForLecon(string leconTitre)
    {
        Console.WriteLine("LeconTitre: " + leconTitre);
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return new List<Exercise>();
        }

        return lecon?.ExercicesList?.Select(DtoMapper.MapExercices);
    }
}
