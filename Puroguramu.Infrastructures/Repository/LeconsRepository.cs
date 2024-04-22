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

    public Task<bool> CreateLecon(string inputTitre)
    {
        var lecon = new Lecons
        {
            IdLecons = Guid.NewGuid().ToString(), Titre = inputTitre, Description = string.Empty, estVisible = true
        };

        // Ajouter la nouvelle leçon à la base de données si elle n'existe pas déjà
        if (_context.Lecons.Any(l => l.Titre == inputTitre))
        {
            return Task.FromResult(false);
        }

        _context.Lecons.Add(lecon);
        _context.SaveChanges();
        return Task.FromResult(true);
    }

    public IEnumerable<Lecon> GetLeconsForCours(string userId)
    {
        var lecona = new List<Lecon>();
        var leconsList = _context.Lecons
            .Include(l => l.ExercicesList)
            .ToList();

        var positionsLecons = _context.PositionLecons.ToList();

        if (leconsList == null)
        {
            return lecona;
        }

        //Trier les leçons par position
        leconsList = leconsList.OrderBy(l => positionsLecons.FirstOrDefault(p => p.Lecons.IdLecons == l.IdLecons)?.Position).ToList();

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

    public async Task<(string, string)> GetNextExerciceAsync(string userId)
    {
        // Charger tous les exercices pour un cours donné
        var lecons = await _context.Lecons
            .Include(l => l.ExercicesList)
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

    public async Task<(string, string)> GetActualExercicesAsync(string userId)
    {
        // Charger tous les exercices pour un cours donné
        var lecons = await _context.Lecons
            .Include(l => l.ExercicesList)
            .ToListAsync();

        // Extraire tous les ID des exercices pour une vérification ultérieure
        var exercicesIds = lecons.SelectMany(l => l.ExercicesList!.Select(e => e.IdExercice)).ToList();

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
                LeconTitre = lecons.FirstOrDefault(l => l.ExercicesList!.Any(e => e.IdExercice == se.Exercice.IdExercice))?.Titre
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

    public Task<bool> UpdateLecon(string leconTitre, string inputTitre, string inputDescription)
    {
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return Task.FromResult(false);
        }

        //Verifier si le titre existe déjà
        if (_context.Lecons.Any(l => l.Titre == inputTitre))
        {
            return Task.FromResult(false);
        }

        lecon.Titre = inputTitre;
        lecon.Description = inputDescription;
        _context.SaveChanges();
        return Task.FromResult(true);
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

    public async Task DeleteLecon(string leconTitre)
    {
        var lecon = await _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefaultAsync(l => l.Titre == leconTitre);

        if (lecon == null)
        {
            return;
        }

        // Supprimer tous les statuts des exercices de la leçon
        var exercicesIds = lecon.ExercicesList.Select(e => e.IdExercice);
        var statutsExercices = _context.StatutExercices.Where(se => exercicesIds.Contains(se.Exercice.IdExercice));
        _context.StatutExercices.RemoveRange(statutsExercices);

        // Supprimer la position de la leçon
        var positionLecon = _context.PositionLecons.FirstOrDefault(p => p.Lecons.IdLecons == lecon.IdLecons);
        if (positionLecon != null)
        {
            _context.PositionLecons.Remove(positionLecon);
        }

        // Supprimer la leçon et ses exercices
        _context.Exercices.RemoveRange(lecon.ExercicesList);
        _context.Lecons.Remove(lecon);

        await _context.SaveChangesAsync();

        // Mettre à jour les positions des leçons restantes
        UpdateLeconPositions();
    }


    public int? GetExercicesFait(string leconTitre)
    {
        // Trouver la leçon par son titre
        var lecon = _context.Lecons
            .Include(l => l.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);

        if (lecon == null || !lecon.ExercicesList.Any())
        {
            // Si aucune leçon ou aucun exercice n'est trouvé, retourner 0
            return 0;
        }

        // Récupérer les IDs de tous les exercices de la leçon
        var exerciceIds = lecon.ExercicesList.Select(e => e.IdExercice).ToList();

        // Trouver les étudiants qui ont réussi tous les exercices de la leçon
        var allPassedStudentsCount = _context.StatutExercices
            .Where(se => exerciceIds.Contains(se.Exercice.IdExercice) && se.Statut == Status.Passed)
            .GroupBy(se => se.Etudiant.Id)
            .Select(group => new { EtudiantId = group.Key, PassedExercisesCount = group.Count() })
            .Count(g => g.PassedExercisesCount == exerciceIds.Count);

        return allPassedStudentsCount;
    }

    public async Task MoveLecon(string leconTitre, string direction)
    {
        // Récupérer la leçon et sa position actuelle
        var currentPosition = await _context.PositionLecons
            .Include(p => p.Lecons)
            .FirstOrDefaultAsync(p => p.Lecons.Titre == leconTitre);

        if (currentPosition == null)
        {
            // Si la position n'est pas trouvée, cela pourrait indiquer un problème de synchronisation des données
            return;
        }

        // Calculer la nouvelle position en fonction de la direction
        var newPosition = direction.ToLower() == "up" ? currentPosition.Position - 1 : currentPosition.Position + 1;

        // Vérifier que la nouvelle position est dans les limites valides
        if (newPosition < 0 || newPosition > _context.PositionLecons.Count())
        {
            // Si la nouvelle position n'est pas valide, ne faites rien
            return;
        }

        // Trouver la leçon à échanger avec
        var targetPosition = await _context.PositionLecons
            .FirstOrDefaultAsync(p => p.Position == newPosition);

        if (targetPosition != null)
        {
            // Échanger les positions
            targetPosition.Position = currentPosition.Position;
            currentPosition.Position = newPosition;

            // Sauvegarder les changements
            _context.Update(currentPosition);
            _context.Update(targetPosition);
            await _context.SaveChangesAsync();
        }
    }

    private void UpdateLeconPositions()
    {
        var remainingLecons = _context.PositionLecons.OrderBy(p => p.Position).ToList();
        for (var i = 0; i < remainingLecons.Count; i++)
        {
            remainingLecons[i].Position = i;
        }

        _context.SaveChanges();
    }
}
