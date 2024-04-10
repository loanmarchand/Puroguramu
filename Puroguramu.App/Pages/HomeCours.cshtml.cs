using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;
using Status = Puroguramu.Domains.Status;

namespace Puroguramu.App.Pages;

public class HomeCours : PageModel
{
    private readonly ICoursRepository _coursRepository;
    private readonly IStatutExerciceRepository _statutExerciceRepository;
    private readonly UserManager<Utilisateurs> _userManager;

    public HomeCours(ICoursRepository coursRepository, IStatutExerciceRepository statutExerciceRepository, UserManager<Utilisateurs> userManager)
    {
        _coursRepository = coursRepository;
        _statutExerciceRepository = statutExerciceRepository;
        _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)]
    public string TitreCours { get; set; }

    public List<Lecon> Lecons { get; set; }


    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        Lecons = _coursRepository.GetLeconsForCours(TitreCours,user.Id).ToList();
    }
}
