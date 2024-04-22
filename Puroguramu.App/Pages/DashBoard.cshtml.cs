using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;
using Role = Puroguramu.Infrastructures.dto.Role;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
public class DashBoard : PageModel
{
    private readonly ILeconsRepository _leconsRepository;
    private readonly UserManager<Utilisateurs> _userManager;

    public DashBoard(ILeconsRepository leconsRepository, UserManager<Utilisateurs> userManager)
    {
        _leconsRepository = leconsRepository;
        _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)] public string TitreCours { get; set; }

    public List<Lecon> Lecons { get; set; }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        ViewData["Role"] = user.Role;
        if (user.Role == Role.Teacher)
        {
            Lecons = _leconsRepository.GetLeconsForCours(user.Id).ToList();
            var nombreEtudiants = _userManager.Users.Count(e => e.Role == Role.Student);
            foreach (var lecon in Lecons)
            {
                lecon.ExercicesFait = _leconsRepository.GetExercicesFait(lecon.Titre);
                lecon.ExercicesTotal = nombreEtudiants;
            }
        }
        else if (user.Role == Role.Student)
        {
            Lecons = _leconsRepository.GetLeconsForCours(user.Id).Where(l => l.EstVisible).ToList();
        }
    }

    public async Task<IActionResult> OnPostProchainExerciceAsync()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var prochainExercice = await _leconsRepository.GetNextExerciceAsync(user.Id);
        Console.WriteLine(prochainExercice);
        if (prochainExercice.Item1 == string.Empty || prochainExercice.Item2 == string.Empty)
        {
            //Afficher un pop-up pas d'exercice disponible
            return RedirectToPage();
        }

        return RedirectToPage("/Exercice", new { Titre = prochainExercice.Item1, LeconTitre = prochainExercice.Item2 });
    }

    public async Task<IActionResult> OnPostReprendreExercicesAsync()
    {
        // Logique pour reprendre les exercices
        var user = _userManager.GetUserAsync(User).Result;
        var prochainExercice = await _leconsRepository.GetActualExercicesAsync(user.Id);
        if (prochainExercice.Item1 == string.Empty || prochainExercice.Item2 == string.Empty)
        {
            //Afficher un pop-up pas d'exercice disponible
            return RedirectToPage();
        }

        return RedirectToPage("/Exercice", new { Titre = prochainExercice.Item1, LeconTitre = prochainExercice.Item2 });
    }

    public async Task<IActionResult> OnPostChangeVisibilityAsync(string leconTitre)
    {
        await _leconsRepository.ToggleVisibilityLecon(leconTitre);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteLeconAsync(string leconTitre)
    {
        await _leconsRepository.DeleteLecon(leconTitre);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostMoveLeconAsync(string leconTitre, string direction)
    {
        await _leconsRepository.MoveLecon(leconTitre, direction);
        return RedirectToPage();
    }
}
