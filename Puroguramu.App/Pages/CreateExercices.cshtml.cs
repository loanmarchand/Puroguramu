using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Repository;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
[Authorize(Policy = "IsTeacher")]
public class CreateExercices : PageModel
{
    private readonly IExercisesRepository _exercisesRepository;

    public CreateExercices(IExercisesRepository exercisesRepository) => _exercisesRepository = exercisesRepository;

    public string ReturnUrl { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    [BindProperty(SupportsGet = true)] public string LeconTitre { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var reslut = await _exercisesRepository.CreateExerciceAsync(LeconTitre, Input.Titre);
        if (!reslut)
        {
            //Afficher un message d'erreur
            ModelState.AddModelError("Input.Titre", "L'exercice existe déjà");
            return Page();
        }

        ReturnUrl = "/EditLecon";
        return RedirectToPage(ReturnUrl, new { LeconTitre });
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le champ titre est requis")]
        [RegularExpression(@"^(?!.*<|>).*.{5,}$", ErrorMessage = "Le titre doit contenir au moins 5 caractères et ne doit pas inclure les symboles <>.")]
        public string Titre { get; set; }
    }
}
