using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;

namespace Puroguramu.Infrastructures.Repository;

public class StatutExerciceRepository : IStatutExerciceRepository
{
    private PurogumaruContext _context;

    public StatutExerciceRepository(PurogumaruContext context)
    {
        _context = context;
    }


    public IEnumerable<StatutExercice> GetStatutExercicesForCoursAndEtudiant(string coursTitre, string etudiantId) => throw new NotImplementedException();
}
