﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

public class IndexModel : PageModel
{
    private readonly IExercisesRepository _exercisesRepository;
    private readonly ILeconsRepository _leconsRepository;
    private readonly SignInManager<Utilisateurs> _signInManager;

    public IndexModel(SignInManager<Utilisateurs> signInManager, ILeconsRepository leconsRepository, IExercisesRepository exercisesRepository)
    {
        _signInManager = signInManager;
        _leconsRepository = leconsRepository;
        _exercisesRepository = exercisesRepository;
    }

    public string? LeconsDispos { get; set; }

    public string? ExosDispos { get; set; }

    public int CountEtudiant { get; set; }

    public IActionResult OnGet()
    {
        // Vérifie si l'utilisateur est déjà connecté
        if (_signInManager.IsSignedIn(User))
        {
            // Redirige vers la page HomePage si l'utilisateur est déjà connecté
            return RedirectToPage("./DashBoard");
        }

        CountEtudiant = _signInManager.UserManager.Users.Count(e => e.Role == Role.Student);
        LeconsDispos = _leconsRepository.GetLecons().Count().ToString();
        ExosDispos = _exercisesRepository.GetExercisesCount().ToString();

        // Continue avec la page Index si l'utilisateur n'est pas connecté
        return Page();
    }
}
