using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;
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
        return lecons.Select(l => DtoMapper.MapLecon(l, null, null)).ToList();
    }

    public Lecon GetLecon(string idLecons, string userId)
    {
        var leconQuery = _context.Lecons
            .Where(l => l.Titre == idLecons)
            .Select(l => new
            {
                Lecon = l,
                Exercices = l.ExercicesList
                    .Where(e => e.EstVisible)
                    .Select(e => new
                    {
                        Exercice = e,
                        // Utilisation de ?? pour définir Status.NotStarted comme valeur par défaut

                        Statut = (dto.Status?)_context.StatutExercices
                            .Where(se => se.Exercice.IdExercice == e.IdExercice && se.Etudiant.Id == userId)
                            .Select(se => se.Statut)
                            .FirstOrDefault() ?? dto.Status.NotStarted,
                    })
                    .ToList(),
            })
            .FirstOrDefault();

        if (leconQuery == null) return null;

        // Assurez-vous que le type correspond exactement à ce que s'attend MapLeconWithStatuts
        var exercicesStatuts = leconQuery.Exercices
            .Select(e => (e.Exercice, (dto.Status?)e.Statut)) // Le Statut est déjà nullable ici
            .ToList();

        // Utilisation du nouveau mapper avec gestion des statuts
        return DtoMapper.MapLeconWithStatuts(leconQuery.Lecon, exercicesStatuts);
    }


    public Exercise GetExercice(string? leconTitre, string? titreExo)
    {
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return new Exercise();
        }

        return DtoMapper.MapExercices(lecon.ExercicesList.FirstOrDefault(e => e.Titre == titreExo)!);
    }

    public string GetExerciceId(string leconTitre, string titre)
    {
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return string.Empty;
        }

        return lecon.ExercicesList.FirstOrDefault(e => e.Titre == titre)?.IdExercice ?? string.Empty;
    }

    public Task CreateLecon(string titreCours, string inputTitre)
    {
        var lecon = new Lecons { IdLecons = Guid.NewGuid().ToString(), Titre = inputTitre, estVisible = true, };
        var cours = _context.Cours.Include(c => c.Lecons).FirstOrDefault(c => c.Titre == titreCours);
        if (cours == null)
        {
            return Task.CompletedTask;
        }

        cours.Lecons.Add(lecon);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public IEnumerable<Lecon> GetLeconsForCours(string nameCours, string userId)
    {
        var lecona = new List<Lecon>();
        var leconsList = _context.Cours
            .Include(c => c.Lecons)!
            .ThenInclude(l => l.ExercicesList)
            .FirstOrDefault(c => c.Titre == nameCours)
            ?.Lecons;

        if (leconsList == null)
        {
            return lecona;
        }

        foreach (var lecon in leconsList)
        {
            if (lecon.ExercicesList == null)
            {
                continue;
            }

            var nombreExercices = lecon.ExercicesList.Count;
            var nombreExercicesFait = _context.StatutExercices
                .Count(se => lecon.ExercicesList.Select(e => e.IdExercice).Contains(se.Exercice.IdExercice) &&
                             se.Etudiant.Id == userId &&
                             se.Statut == dto.Status.Passed);

            lecona.Add(DtoMapper.MapLecon(lecon, nombreExercicesFait, nombreExercices));
        }

        return lecona;
    }
}
