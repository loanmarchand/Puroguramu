﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;
using Status = Puroguramu.Domains.Status;
using StatutExercice = Puroguramu.Infrastructures.dto.StatutExercice;

namespace Puroguramu.Infrastructures.Repository;

public class StatutExerciceRepository : IStatutExerciceRepository
{
    private PurogumaruContext _context;
    private readonly UserManager<Utilisateurs> _userManager;

    public StatutExerciceRepository(PurogumaruContext context, UserManager<Utilisateurs> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public Status? GetStatut(string getExerciceId, string getUserId)
    {
        dto.Status? stat = _context.StatutExercices
            .Include(s => s.Exercice)
            .FirstOrDefault(s => s.Exercice.IdExercice == getExerciceId && s.Etudiant.Id == getUserId)?.Statut;

        // Implement mapping from dto.Status to Domains.Status
        if (stat.HasValue)
        {
            return (Status)stat.Value; // Cast if the names and values of the enums are exactly the same
        }

        return null;
    }


    public Task CreateStatut(string getExerciceId, string getUserId)
    {
        var statut = new StatutExercice
        {
            IdStatutExercice = Guid.NewGuid().ToString(),
            Exercice = _context.Exercices.Find(getExerciceId),
            Etudiant = _userManager.FindByIdAsync(getUserId).Result,
            Statut = dto.Status.Started,
        };

        _context.StatutExercices.Add(statut);
        return _context.SaveChangesAsync();
    }

    public Task UpdateStatut(string getExerciceId, string getUserId, ExerciseStatus resultStatus)
    {
        var status = resultStatus switch
        {
            ExerciseStatus.Failed => dto.Status.Failed,
            ExerciseStatus.NotStarted => dto.Status.NotStarted,
            ExerciseStatus.Started => dto.Status.Started,
            ExerciseStatus.Passed => dto.Status.Passed,
            _ => throw new ArgumentOutOfRangeException(nameof(resultStatus), resultStatus, null)
        };

        var statut = _context.StatutExercices
            .Include(s => s.Exercice)
            .FirstOrDefault(s => s.Exercice.IdExercice == getExerciceId && s.Etudiant.Id == getUserId)!;

        statut.Statut = status;
        _context.StatutExercices.Update(statut);
        return _context.SaveChangesAsync();
    }

    public Task UpdateSolutionTempo(string getExerciceId, string getUserId, string proposal)
    {
        var statut = _context.StatutExercices
            .Include(s => s.Exercice)
            .FirstOrDefault(s => s.Exercice.IdExercice == getExerciceId && s.Etudiant.Id == getUserId)!;

        statut.SolutionTempo = proposal;
        _context.StatutExercices.Update(statut);
        return _context.SaveChangesAsync();
    }

    public Task<string?> GetSolutionTempo(string getExerciceId, string getUserId) =>
        Task.FromResult(_context.StatutExercices
            .Include(s => s.Exercice)
            .FirstOrDefault(s => s.Exercice.IdExercice == getExerciceId && s.Etudiant.Id == getUserId)?.SolutionTempo);
}
