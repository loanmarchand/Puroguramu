using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.data;
using Puroguramu.Infrastructures.dto;
using Status = Puroguramu.Domains.Status;

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
        dto.StatutExercice statut = new dto.StatutExercice
        {
            IdStatutExercice = default(Guid).ToString(),
            Exercice = _context.Exercices.Find(getExerciceId),
            Etudiant = _userManager.FindByIdAsync(getUserId).Result,
            Statut = dto.Status.Started,
        };

        _context.StatutExercices.Add(statut);
        return _context.SaveChangesAsync();
    }
}
