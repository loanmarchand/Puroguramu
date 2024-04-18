using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Puroguramu.App.Pages;

public class EditExercice : PageModel
{
    public string ReturnUrl { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public void OnGet()
    {
    }

    public class InputModel
    {
        public string Titre { get; set; }

        public string Enonce { get; set; }

        public string Modele { get; set; }

        public string Solution { get; set; }
    }
}
