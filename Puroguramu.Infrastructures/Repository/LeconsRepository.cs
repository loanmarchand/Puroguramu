using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;
using Puroguramu.Infrastructures.Mapper;
using Status = Puroguramu.Infrastructures.dto.Status;

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

    public Lecon? GetLeconWithStatuts(string idLecons, string userId)
    {
        var leconQuery = _context.Lecons
            .Where(l => l.Titre == idLecons)
            .Select(l => new
            {
                Lecon = l,
                Exercices = l.ExercicesList!
                    .Where(e => e.EstVisible)
                    .Select(e => new
                    {
                        Exercice = e,

                        // Utilisation de ?? pour définir Status.NotStarted comme valeur par défaut
                        Statut = (Status?)_context.StatutExercices
                            .Where(se => se.Exercice.IdExercice == e.IdExercice && se.Etudiant.Id == userId)
                            .Select(se => se.Statut)
                            .FirstOrDefault() ?? Status.NotStarted,
                    })
                    .ToList(),
            })
            .FirstOrDefault();

        if (leconQuery == null)
        {
            return null;
        }

        // Assurez-vous que le type correspond exactement à ce que s'attend MapLeconWithStatuts
        var exercicesStatuts = leconQuery.Exercices
            .Select(e => (e.Exercice, (Status?)e.Statut)) // Le Statut est déjà nullable ici
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

        return DtoMapper.MapExercices(lecon.ExercicesList!.FirstOrDefault(e => e.Titre == titreExo)!);
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

        return lecon.ExercicesList!.FirstOrDefault(e => e.Titre == titre)?.IdExercice ?? string.Empty;
    }

    public Task CreateLecon(string titreCours, string inputTitre)
    {
        var lecon = new Lecons { IdLecons = Guid.NewGuid().ToString(), Titre = inputTitre, estVisible = true, };
        var cours = _context.Cours.Include(c => c.Lecons).FirstOrDefault(c => c.Titre == titreCours);
        if (cours == null)
        {
            return Task.CompletedTask;
        }

        cours.Lecons?.Add(lecon);
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
                             se.Statut == Status.Passed);

            lecona.Add(DtoMapper.MapLecon(lecon, nombreExercicesFait, nombreExercices));
        }

        return lecona;
    }

    public async Task<(string, string)> GetNextExerciceAsync(string titreCours, string userId)
    {
        // Charger tous les exercices pour un cours donné
        var lecons = await _context.Cours
            .Where(c => c.Titre == titreCours)
            .SelectMany(c => c.Lecons!).Include(lecons => lecons.ExercicesList)
            .ToListAsync();

        // Charger les statuts séparément
        var statuts = await _context.StatutExercices
            .Where(se => se.Etudiant.Id == userId)
            .ToListAsync();

        // Corréler les données manuellement
        var exercices = lecons
            .SelectMany(l => l.ExercicesList!.Select(e => new { LeconTitre = l.Titre, Exercice = e }))
            .ToList();

        // Trouver les exercices avec statuts
        var exercicesAvecStatuts = from e in exercices
            join s in statuts on e.Exercice.IdExercice equals s.Exercice.IdExercice into es
            from s in es.DefaultIfEmpty()
            select new { e.LeconTitre, e.Exercice, Statut = s?.Statut ?? Status.NotStarted };

        // Appliquer la logique de sélection du prochain exercice
        var avecStatuts = exercicesAvecStatuts.ToList();
        var lastFinished = avecStatuts.FirstOrDefault(e => e.Statut == Status.Passed);
        if (lastFinished != null)
        {
            var next = exercices.SkipWhile(e => e.Exercice.IdExercice != lastFinished.Exercice.IdExercice).Skip(1).FirstOrDefault();
            if (next != null)
            {
                return (next.Exercice.Titre, next.LeconTitre);
            }
        }

        var firstStarted = avecStatuts.FirstOrDefault(e => e.Statut == Status.Started);
        if (firstStarted != null)
        {
            return (firstStarted.Exercice.Titre, firstStarted.LeconTitre);
        }

        var firstExercise = exercices.FirstOrDefault();
        return firstExercise != null ? (firstExercise.Exercice.Titre, firstExercise.LeconTitre) : (string.Empty, string.Empty);
    }

    public async Task<(string, string)> GetActualExercicesAsync(string titreCours, string userId)
    {
        // Charger tous les exercices pour un cours donné
        var lecons = await _context.Cours
            .Where(c => c.Titre == titreCours)
            .Include(c => c.Lecons)!
            .ThenInclude(l => l.ExercicesList)
            .ToListAsync();

        // Extraire tous les ID des exercices pour une vérification ultérieure
        var exercicesIds = lecons.SelectMany(c => c.Lecons!)
            .SelectMany(l => l.ExercicesList!)
            .Select(e => e.IdExercice)
            .ToList();

        // Charger les statuts des exercices pour cet utilisateur qui sont dans la liste des IDs chargés
        var statuts = await _context.StatutExercices
            .Where(se => se.Etudiant.Id == userId && exercicesIds.Contains(se.Exercice.IdExercice))
            .Include(se => se.Exercice)
            .ToListAsync();

        // Recherche du dernier exercice commencé par l'utilisateur
        var dernierExerciceCommence = statuts
            .Where(se => se.Statut == Status.Started)
            .OrderByDescending(se => se.Exercice.IdExercice)
            .Select(se => new
            {
                ExerciceId = se.Exercice.IdExercice,
                ExerciceTitre = se.Exercice.Titre,
                LeconTitre = lecons.SelectMany(c => c.Lecons!)
                    .FirstOrDefault(l => l.ExercicesList!.Any(e => e.IdExercice == se.Exercice.IdExercice))
                    ?.Titre,
            })
            .FirstOrDefault();

        return (dernierExerciceCommence != null
            ? (dernierExerciceCommence.ExerciceTitre, dernierExerciceCommence.LeconTitre)
            : (string.Empty, string.Empty))!;
    }

    public Lecon GetLecon(string leconTitre)
    {
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        return DtoMapper.MapLecon(lecon, null, null);
    }

    public void UpdateLecon(string leconTitre, string inputTitre, string inputDescription)
    {
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return;
        }

        lecon.Titre = inputTitre;
        lecon.Description = inputDescription;
        _context.SaveChanges();
    }

    public Task ToggleVisibilityLecon(string leconTitre)
    {
        var lecon = _context.Lecons
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return Task.CompletedTask;
        }

        lecon.estVisible = !lecon.estVisible;
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteLecon(string leconTitre)
    {
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return Task.CompletedTask;
        }

        _context.Lecons.Remove(lecon);
        _context.SaveChanges();
        return Task.CompletedTask;
    }
}
