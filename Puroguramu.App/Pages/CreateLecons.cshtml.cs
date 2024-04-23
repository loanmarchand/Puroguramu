using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Repository;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
[Authorize(Policy = "IsTeacher")]
public class CreateLecons : PageModel
{
    private ILeconsRepository _leconsRepository;

    public CreateLecons(ILeconsRepository leconsRepository) => _leconsRepository = leconsRepository;

    public string ReturnUrl { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine("Titre: " + Input.Titre);
            var add = await _leconsRepository.CreateLecon(Input.Titre);
            if (add)
            {
                return RedirectToPage("/Dashboard");
            }

            ModelState.AddModelError("Input.Titre", "Une leçon avec ce titre existe déjà.");
        }

        return Page();
    }

    public class InputModel
    {
        [Required(ErrorMessage = "Le champ titre est requis")]
        [RegularExpression(@"^(?!.*<|>).*.{5,}$", ErrorMessage = "Le titre doit contenir au moins 5 caractères et ne doit pas inclure les symboles <>.")]
        public string Titre { get; set; }
    }
}
