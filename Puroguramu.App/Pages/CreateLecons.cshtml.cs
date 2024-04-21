using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Repository;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
public class CreateLecons : PageModel
{
    private ILeconsRepository _leconsRepository;

    public CreateLecons(ILeconsRepository leconsRepository) => _leconsRepository = leconsRepository;

    public string ReturnUrl { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    [BindProperty(SupportsGet = true)] public string TitreCours { get; set; }

    public class InputModel
    {
        public string Titre { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var add = await _leconsRepository.CreateLecon(TitreCours, Input.Titre);
            if (add)
            {
                return RedirectToPage("/HomeCours", new { TitreCours });
            }
        }

        // If we get here, something went wrong, afficher la page avec les erreurs, le titre existe déjà //TODO
        return Page();
    }
}
