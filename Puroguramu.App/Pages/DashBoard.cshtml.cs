using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

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
        Lecons = _leconsRepository.GetLeconsForCours(TitreCours, user.Id).ToList();
    }

    public async Task<IActionResult> OnPostProchainExerciceAsync()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var prochainExercice = await _leconsRepository.GetNextExerciceAsync(TitreCours, user.Id);
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
        var prochainExercice = await _leconsRepository.GetActualExercicesAsync(TitreCours, user.Id);
        if (prochainExercice.Item1 == string.Empty || prochainExercice.Item2 == string.Empty)
        {
            //Afficher un pop-up pas d'exercice disponible

            return RedirectToPage();
        }

        return RedirectToPage("/Exercice", new { Titre = prochainExercice.Item1, LeconTitre = prochainExercice.Item2 });
    }
}
