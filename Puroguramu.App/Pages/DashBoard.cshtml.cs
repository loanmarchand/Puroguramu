using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;
using Status = Puroguramu.Domains.Status;

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

    [BindProperty(SupportsGet = true)]
    public string TitreCours { get; set; }

    public List<Lecon> Lecons { get; set; }


    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        Lecons = _leconsRepository.GetLeconsForCours(TitreCours,user.Id).ToList();
    }

    public Task<IActionResult> OnPostProchainExerciceAsync()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var prochainExercice = _leconsRepository.GetNextExerciceAsync(TitreCours, user.Id);
        return Task.FromResult<IActionResult>(RedirectToPage("/Exercice", new { Titre = prochainExercice.Result.Item1, LeconTitre = prochainExercice.Result.Item2}));
    }

    public Task<IActionResult> OnPostReprendreExercicesAsync()
    {
        // Logique pour reprendre les exercices
        return Task.FromResult<IActionResult>(RedirectToPage("/ReprendreExercices", new { TitreCours = TitreCours }));
    }
}
