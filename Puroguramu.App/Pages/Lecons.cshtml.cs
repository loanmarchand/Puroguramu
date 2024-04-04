using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;

namespace Puroguramu.App.Pages;

public class Lecons : PageModel
{
    private readonly ILeconsRepository _leconsRepository;

    public Lecons(ILeconsRepository leconsRepository)
    {
        _leconsRepository = leconsRepository;
    }

    public void OnGet()
    {
        Lecon = _leconsRepository.GetLecon(LeconTitre);
    }

    [BindProperty(SupportsGet = true)]
    public string? LeconTitre { get; set; }
    public Lecon Lecon{ get; set; }
}
