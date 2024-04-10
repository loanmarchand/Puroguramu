﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Repository;

namespace Puroguramu.App.Pages;

public class CreateLecons : PageModel
{
    private ILeconsRepository _leconsRepository;

    public CreateLecons(ILeconsRepository leconsRepository) => _leconsRepository = leconsRepository;

    public string ReturnUrl { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }
    [BindProperty(SupportsGet = true)]
    public string TitreCours { get; set; }

    public class InputModel
    {
        public string Titre { get; set; }
    }

    public void OnGet()
    {

    }

    public async void OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            await _leconsRepository.CreateLecon(TitreCours, Input.Titre);
            ReturnUrl = "/HomeCours/" + TitreCours;
        }

    }
}
