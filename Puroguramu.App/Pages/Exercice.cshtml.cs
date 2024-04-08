using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.App.Pages;

public class Exercice : PageModel
{
    private readonly IAssessExercise _assessor;
    private readonly ILeconsRepository _leconsRepository;
    private readonly IStatutExerciceRepository _statutExerciceRepository;
    private readonly UserManager<Utilisateurs> _userManager;

    private ExerciseResult? _result;

    [BindProperty]
    public string Proposal { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string Titre { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string LeconTitre { get; set; } = string.Empty;

    public string ExerciseResultStatus
        => _result?.Status switch
        {
            ExerciseStatus.NotStarted => "Not Started",
            ExerciseStatus.Started => "Started",
            ExerciseStatus.Passed => "Succeeded",
            ExerciseStatus.Failed => "Failed",
            _ => "Unknown"
        };

    public IEnumerable<TestResultViewModel> TestResult
        => _result
            ?.TestResults
            ?.Select(result => new TestResultViewModel(result)) ?? Array.Empty<TestResultViewModel>();

    public Exercise Exercices { get; set; }

    public Exercice(IAssessExercise assessor, ILeconsRepository leconsRepository, IStatutExerciceRepository statutExerciceRepository, UserManager<Utilisateurs> userManager)
    {
        _assessor = assessor;
        _leconsRepository = leconsRepository;
        _statutExerciceRepository = statutExerciceRepository;
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        Exercices = _leconsRepository.GetExercice(LeconTitre, Titre);
        var statut =  _statutExerciceRepository.GetStatut(_leconsRepository.GetExerciceId(LeconTitre, Titre),_userManager.GetUserId(User));
        if (statut == null)
        {
            await _statutExerciceRepository.CreateStatut(_leconsRepository.GetExerciceId(LeconTitre, Titre), _userManager.GetUserId(User));
        }

        _result = await _assessor.StubForExercise(_leconsRepository.GetExerciceId(LeconTitre, Titre));
        Proposal = _result.Proposal;
    }

    public async Task OnPostAsync()
    {
        Exercices = _leconsRepository.GetExercice(LeconTitre, Titre);
        _result = await _assessor.Assess(_leconsRepository.GetExerciceId(LeconTitre, Titre), Proposal);
        await _statutExerciceRepository.UpdateStatut(_leconsRepository.GetExerciceId(LeconTitre, Titre), _userManager.GetUserId(User), _result.Status);
    }
}

public record TestResultViewModel(TestResult Result)
{
    public string Status
        => Result.Status.ToString();

    public string Label
        => Result.Label;

    public bool HasError
        => Result.Status != TestStatus.Passed;

    public string ErrorMessage
        => Result.ErrorMessage;
}


