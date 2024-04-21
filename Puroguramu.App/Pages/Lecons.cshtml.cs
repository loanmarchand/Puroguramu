using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
public class Lecons : PageModel
{
    private readonly ILeconsRepository _leconsRepository;
    private readonly UserManager<Utilisateurs> _userManager;

    public Lecons(ILeconsRepository leconsRepository, UserManager<Utilisateurs> userManager)
    {
        _leconsRepository = leconsRepository;
        _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)] public string? LeconTitre { get; set; }

    public Lecon? Lecon { get; set; }

    public async void OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        ViewData["Role"] = user.Role;
        Lecon = _leconsRepository.GetLeconWithStatuts(LeconTitre!, _userManager.GetUserId(User));
    }
}
