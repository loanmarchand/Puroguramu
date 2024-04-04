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

    public Lecon GetLecon(string idLecons) => throw new NotImplementedException();

    public void AddLecon(Lecon lecon) => throw new NotImplementedException();

    public void UpdateLecon(Lecon lecon) => throw new NotImplementedException();

    public void DeleteLecon(Lecon lecon) => throw new NotImplementedException();

}
