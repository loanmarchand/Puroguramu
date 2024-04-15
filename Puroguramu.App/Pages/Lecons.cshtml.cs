using System.Collections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

public class Lecons : PageModel
{
    private readonly ILeconsRepository _leconsRepository;
    private readonly UserManager<Utilisateurs> _userManager;

    public Lecons(ILeconsRepository leconsRepository, UserManager<Utilisateurs> userManager)
    {
        _leconsRepository = leconsRepository;
        _userManager = userManager;
    }

    public void OnGet() => Lecon = _leconsRepository.GetLecon(LeconTitre, _userManager.GetUserId(User));

    [BindProperty(SupportsGet = true)]
    public string? LeconTitre { get; set; }

    public Lecon? Lecon { get; set; }
}
