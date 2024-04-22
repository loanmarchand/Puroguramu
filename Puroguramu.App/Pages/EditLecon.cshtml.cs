using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Repository;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
public class EditLecon : PageModel
{
    private readonly ILeconsRepository _leconsRepository;

    public EditLecon(ILeconsRepository leconsRepository) => _leconsRepository = leconsRepository;

    public string ReturnUrl { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    [BindProperty(SupportsGet = true)] public string LeconTitre { get; set; }

    public void OnGet()
    {
        var lecon = _leconsRepository.GetLecon(LeconTitre);
        Input = new InputModel { Titre = lecon.Titre, Description = lecon.Description };
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _leconsRepository.UpdateLecon(LeconTitre, Input.Titre, Input.Description);

        return RedirectToPage("/DashBoard");
    }

    public class InputModel
    {
        public string Titre { get; set; }

        public string Description { get; set; }
    }
}
