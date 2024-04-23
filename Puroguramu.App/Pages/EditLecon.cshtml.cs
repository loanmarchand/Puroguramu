using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
public class EditLecon : PageModel
{
    private readonly IExercisesRepository _exercisesRepository;
    private readonly ILeconsRepository _leconsRepository;
    private readonly UserManager<Utilisateurs> _userManager;


    public EditLecon(ILeconsRepository leconsRepository, UserManager<Utilisateurs> userManager, IExercisesRepository exercisesRepository)
    {
        _leconsRepository = leconsRepository;
        _userManager = userManager;
        _exercisesRepository = exercisesRepository;
    }

    public string ReturnUrl { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    [BindProperty(SupportsGet = true)] public string LeconTitre { get; set; }
    public Lecon Lecon { get; set; }

    public async void OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        ViewData["Role"] = user.Role;
        Lecon = _leconsRepository.GetLecon(LeconTitre);
        Input = new InputModel { Titre = Lecon.Titre, Description = Lecon.Description };
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var verif = _leconsRepository.UpdateLecon(LeconTitre, Input.Titre, Input.Description);
        if (!verif.Result)
        {
            //TODO: Afficher un message d'erreur
            return RedirectToPage();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteExerciceAsync(string leconTitre, string exerciceTitre)
    {
        var verif = await _exercisesRepository.DeleteExercice(leconTitre, exerciceTitre);
        if (!verif)
        {
            //TODO: Afficher un message d'erreur
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostChangeVisibilityAsync(string leconTitre, string exerciceTitre)
    {
        var verif = await _exercisesRepository.ChangeVisibility(leconTitre, exerciceTitre);
        if (!verif)
        {
            //TODO: Afficher un message d'erreur
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostMoveExerciceAsync(string leconTitre, string exerciceTitre, string direction)
    {
        var verif = await _exercisesRepository.MoveExercice(leconTitre, exerciceTitre, direction);
        if (!verif)
        {
            //TODO: Afficher un message d'erreur
        }

        return RedirectToPage();
    }

    public class InputModel
    {
        public string Titre { get; set; }

        public string Description { get; set; }
    }
}
