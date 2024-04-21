using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Repository;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
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

        await _exercisesRepository.CreateExerciceAsync(LeconTitre, Input.Titre);

        return RedirectToPage(ReturnUrl);
    }

    public class InputModel
    {
        public string Titre { get; set; }
    }
}
