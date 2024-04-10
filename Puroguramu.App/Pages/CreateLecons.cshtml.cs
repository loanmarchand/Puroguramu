using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Puroguramu.App.Pages;

public class CreateLecons : PageModel
{

    public string ReturnUrl { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public string Titre { get; set; }
    }

    public void OnGet()
    {

    }
}
