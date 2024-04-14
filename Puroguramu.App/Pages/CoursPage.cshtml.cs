using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

public class CoursPage : PageModel
{
    private readonly ICoursRepository _coursRepository;
    private readonly SignInManager<Utilisateurs> _signInManager; // Ajoutez cette ligne

    // Injectez SignInManager dans le constructeur
    public CoursPage(ICoursRepository coursRepository, SignInManager<Utilisateurs> signInManager)
    {
        _coursRepository = coursRepository;
        _signInManager = signInManager;
    }

    public IEnumerable<Cour> Cours { get; set; }

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
        Cours = _coursRepository.GetCours();

        // Continuez avec la page normalement pour les utilisateurs non connectés
        return Page();
    }
}
