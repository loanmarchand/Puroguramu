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
            .ToList();

        return coursList.Select(DtoMapper.MapCours).ToList();
    }

}

