using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Puroguramu.App.Pages;

public class EditLecon : PageModel
{

    public string ReturnUrl { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public string Titre { get; set; }

        public string Description { get; set; }
    }

    public void OnGet()
    {

    }
}

