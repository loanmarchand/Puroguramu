using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;
using Puroguramu.Infrastructures.Mapper;

namespace Puroguramu.Infrastructures.Repository;

public class ExercicesRepository : IExercisesRepository
{
    private readonly PurogumaruContext _context;

    public ExercicesRepository(PurogumaruContext context) => _context = context;

    public int GetExercisesCount() => _context.Exercices.Count();

    public Exercise GetExercise(string exerciseId) => DtoMapper.MapExercices(_context.Exercices.Find(exerciseId)!);

    public Task<bool> CreateExerciceAsync(string leconTitre, string inputTitre)
    {
        var lecon = _context.Lecons
            .Include(e => e.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return Task.FromResult(false);
        }

        var exercice = new Exercices { IdExercice = Guid.NewGuid().ToString(), Titre = inputTitre, EstVisible = true };
        //Vérifier si l'exercice existe déjà dans la leçon
        if (lecon.ExercicesList?.Any(e => e.Titre == exercice.Titre) == true)
        {
            return Task.FromResult(false);
        }

        lecon.ExercicesList?.Add(exercice);

        // Ajouter la position de l'exercice
        var position = new PositionExercices { IdPositionExercices = Guid.NewGuid().ToString(), Exercices = exercice, Position = lecon.ExercicesList.Count - 1 };
        _context.PositionExercices.Add(position);
        _context.SaveChanges();
        return Task.FromResult(true);
    }

    public async Task<bool> DeleteExercice(string leconTitre, string exerciceTitre)
    {
        var lecon = _context.Lecons
            .Include(e => e.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return false;
        }

        var exercice = lecon.ExercicesList?.FirstOrDefault(e => e.Titre == exerciceTitre);
        if (exercice == null)
        {
            return false;
        }

        // Supprimer les statuts de l'exercice
        var statuts = _context.StatutExercices.Where(s => s.Exercice.IdExercice == exercice.IdExercice);
        _context.StatutExercices.RemoveRange(statuts);

        // Supprimer la position de l'exercice
        var positionToRemove = _context.PositionExercices.FirstOrDefault(p => p.Exercices.IdExercice == exercice.IdExercice);
        if (positionToRemove != null)
        {
            _context.PositionExercices.Remove(positionToRemove);
        }

        // Supprimer l'exercice de la liste des exercices de la leçon
        lecon.ExercicesList.Remove(exercice);

        await _context.SaveChangesAsync();

        // Mettre à jour les positions des autres exercices de la leçon
        await UpdateExercisesPositions(lecon);

        return true;
    }

    public Task<bool> ChangeVisibility(string leconTitre, string exerciceTitre)
    {
        var lecon = _context.Lecons
            .Include(e => e.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return Task.FromResult(false);
        }

        var exercice = lecon.ExercicesList?.FirstOrDefault(e => e.Titre == exerciceTitre);
        if (exercice == null)
        {
            return Task.FromResult(false);
        }

        exercice.EstVisible = !exercice.EstVisible;

        _context.SaveChanges();
        return Task.FromResult(true);
    }

    public Task<bool> MoveExercice(string leconTitre, string exerciceTitre, string directon)
    {
        var lecon = _context.Lecons
            .Include(e => e.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return Task.FromResult(false);
        }

        var exercice = lecon.ExercicesList?.FirstOrDefault(e => e.Titre == exerciceTitre);
        if (exercice == null)
        {
            return Task.FromResult(false);
        }

        var position = _context.PositionExercices.FirstOrDefault(p => p.Exercices.IdExercice == exercice.IdExercice);
        if (position == null)
        {
            return Task.FromResult(false);
        }

        var newPosition = directon == "up" ? position.Position - 1 : position.Position + 1;
        if (newPosition < 0 || newPosition >= lecon.ExercicesList.Count)
        {
            return Task.FromResult(false);
        }

        var otherPosition = _context.PositionExercices.FirstOrDefault(p => p.Position == newPosition);
        if (otherPosition == null)
        {
            return Task.FromResult(false);
        }

        otherPosition.Position = position.Position;
        position.Position = newPosition;

        _context.SaveChanges();
        return Task.FromResult(true);
    }

    public Exercise GetExercise(string leconTitre, string exerciceTitre)
    {
        var lecon = _context.Lecons
            .Include(e => e.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return new Exercise();
        }

        var exercice = lecon.ExercicesList?.FirstOrDefault(e => e.Titre == exerciceTitre);
        return DtoMapper.MapExercices(exercice);
    }

    public async Task<bool> UpdateExercise(Exercise exercice, string tempTitre, string leconTitre)
    {
        var lecon = _context.Lecons
            .Include(e => e.ExercicesList)
            .FirstOrDefault(l => l.Titre == leconTitre);
        if (lecon == null)
        {
            return false;
        }

        var exerciceToUpdate = lecon.ExercicesList?.FirstOrDefault(e => e.Titre == tempTitre);
        if (exerciceToUpdate == null)
        {
            return false;
        }

        if (tempTitre != exercice.Titre)
        {
            // Vérifier si le titre de l'exercice n'existe pas déjà
            if (lecon.ExercicesList.Any(e => e.Titre == exercice.Titre))
            {
                return false;
            }
        }


        exerciceToUpdate.Titre = exercice.Titre;
        exerciceToUpdate.Enonce = exercice.Enonce;
        exerciceToUpdate.Modele = exercice.Modele;
        exerciceToUpdate.Solution = exercice.Solution;
        exerciceToUpdate.Difficulte = exercice.Difficulte;

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task UpdateExercisesPositions(Lecons lecon)
    {
        // Récupérer les positions actuelles des exercices de la leçon
        var exercicesIds = lecon.ExercicesList.Select(e => e.IdExercice).ToList();
        var positions = _context.PositionExercices.Where(p => exercicesIds.Contains(p.Exercices.IdExercice)).ToList();

        // Mettre à jour les positions des exercices
        for (var i = 0; i < positions.Count; i++)
        {
            positions[i].Position = i;
        }

        await _context.SaveChangesAsync();
    }
}
