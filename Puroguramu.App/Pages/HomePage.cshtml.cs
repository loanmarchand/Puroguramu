using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

public class HomePage : PageModel
{
    private readonly ILeconsRepository _leconsRepository;
    private readonly SignInManager<Utilisateurs> _signInManager; // Ajoutez cette ligne

    // Injectez SignInManager dans le constructeur
    public HomePage(ILeconsRepository leconsRepository, SignInManager<Utilisateurs> signInManager)
    {
        _leconsRepository = leconsRepository;
        _signInManager = signInManager;
    }

    public IEnumerable<Lecon> Lecons { get; private set; }
    public int NombreExoFait { get; set; }
    public int NombreExoTotal { get; set; }

    public IActionResult OnGet()
    {
        // Vérifie si l'utilisateur est déjà connecté
        if (!_signInManager.IsSignedIn(User))
        {
            // Si l'utilisateur est connecté, redirigez-le
            // Vous pouvez changer la redirection selon vos besoins
            return RedirectToPage("./Index");
        }

        // Chargez les leçons seulement si l'utilisateur n'est pas connecté
        Lecons = _leconsRepository.GetLecons();

        // Continuez avec la page normalement pour les utilisateurs non connectés
        return Page();
    }
}
