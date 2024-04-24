using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
[Authorize(Policy = "IsTeacher")]
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

    [BindProperty]
    public InputModel Input { get; set; }

    [BindProperty(SupportsGet = true)]
    public string LeconTitre { get; set; }

    public Lecon Lecon { get; set; }

    public async void OnGetAsync()
    {
        Lecon = _leconsRepository.GetLecon(LeconTitre);
        Input = new InputModel { Titre = Lecon.Titre, Description = Lecon.Description };
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            var verif = _leconsRepository.UpdateLecon(LeconTitre, Input.Titre, Input.Description); //TODO: possibilité de changer que le titre ou la description
            if (!verif.Result)
            {
                ModelState.AddModelError("Input.Titre", "Le titre de la leçon existe déjà");
                return Page();
            }

            return RedirectToPage("/EditLecon", new { LeconTitre = Input.Titre });
        }
        Lecon = _leconsRepository.GetLecon(LeconTitre);

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteExerciceAsync(string leconTitre, string exerciceTitre)
    {
        var verif = await _exercisesRepository.DeleteExercice(leconTitre, exerciceTitre);
        if (!verif)
        {
            //TODO: Afficher un message d'erreur
            /*Lecon = _leconsRepository.GetLecon(LeconTitre);
            return Page();*/
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostChangeVisibilityAsync(string leconTitre, string exerciceTitre)
    {
        var verif = await _exercisesRepository.ChangeVisibility(leconTitre, exerciceTitre);
        if (!verif)
        {
            //TODO: Afficher un message d'erreur
            /*Lecon = _leconsRepository.GetLecon(LeconTitre);
            return Page();*/
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostMoveExerciceAsync(string leconTitre, string exerciceTitre, string direction)
    {
        var verif = await _exercisesRepository.MoveExercice(leconTitre, exerciceTitre, direction);
        if (!verif)
        {
            //TODO: Afficher un message d'erreur
            /*Lecon = _leconsRepository.GetLecon(LeconTitre);
            return Page();*/
        }

        return RedirectToPage();
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le champ titre est requis")]
        [RegularExpression(@"^(?!.*<|>).*.{5,}$", ErrorMessage = "Le titre doit contenir au moins 5 caractères et ne doit pas inclure les symboles <>.")]
        public string Titre { get; set; }

        [RegularExpression(@"^(?!.*<|>).*.{5,}$", ErrorMessage = "La description doit contenir au moins 5 caractères et ne doit pas inclure les symboles <>.")]
        public string Description { get; set; }
    }
}
