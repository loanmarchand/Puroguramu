using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;
using Puroguramu.Infrastructures.dto;
using Status = Puroguramu.Domains.Status;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
public class Exercice : PageModel
{
    private readonly IAssessExercise _assessor;
    private readonly ILeconsRepository _leconsRepository;
    private readonly IStatutExerciceRepository _statutExerciceRepository;
    private readonly UserManager<Utilisateurs> _userManager;

    private ExerciseResult? _result;

    public Exercice(IAssessExercise assessor, ILeconsRepository leconsRepository, IStatutExerciceRepository statutExerciceRepository, UserManager<Utilisateurs> userManager)
    {
        _assessor = assessor;
        _leconsRepository = leconsRepository;
        _statutExerciceRepository = statutExerciceRepository;
        _userManager = userManager;
    }

    [BindProperty] public string Proposal { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)] public string Titre { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)] public string LeconTitre { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)] public int? ShowSolution { get; set; } = null;

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

    public async Task OnGetAsync()
    {
        Exercices = _leconsRepository.GetExercice(LeconTitre, Titre);
        var statut = _statutExerciceRepository.GetStatut(_leconsRepository.GetExerciceId(LeconTitre, Titre), _userManager.GetUserId(User));
        if (statut == null)
        {
            await _statutExerciceRepository.CreateStatut(_leconsRepository.GetExerciceId(LeconTitre, Titre), _userManager.GetUserId(User));
        }

        if (ShowSolution is 1)
        {
            Proposal = Exercices.Solution;

            if (statut != Status.Passed)
            {
                await _statutExerciceRepository.UpdateStatut(_leconsRepository.GetExerciceId(LeconTitre, Titre), _userManager.GetUserId(User), ExerciseStatus.Failed);
            }
        }
        else
        {
            _result = await _assessor.StubForExercise(_leconsRepository.GetExerciceId(LeconTitre, Titre));
            if (statut == null)
            {
                Proposal = Exercices.Stub;
            }
            else
            {
                var prop = await _statutExerciceRepository.GetSolutionTempo(_leconsRepository.GetExerciceId(LeconTitre, Titre), _userManager.GetUserId(User));
                Console.WriteLine("prop:" + prop);
                Proposal = prop ?? Exercices.Stub;
            }
        }
    }

    public async Task OnPostAsync()
    {
        Exercices = _leconsRepository.GetExercice(LeconTitre, Titre);
        _result = await _assessor.Assess(_leconsRepository.GetExerciceId(LeconTitre, Titre), Proposal);
        if (ShowSolution is not 1)
        {
            await _statutExerciceRepository.UpdateStatut(_leconsRepository.GetExerciceId(LeconTitre, Titre), _userManager.GetUserId(User), _result.Status);
            await _statutExerciceRepository.UpdateSolutionTempo(_leconsRepository.GetExerciceId(LeconTitre, Titre), _userManager.GetUserId(User), Proposal);
        }
    }

    public async Task<IActionResult> OnPostProchainExerciceAsync()
    {
        var user = _userManager.GetUserAsync(User).Result;
        var prochainExercice = await _leconsRepository.GetNextExerciceAsync(user.Id);
        Console.WriteLine(prochainExercice);
        if (prochainExercice.Item1 == null || prochainExercice.Item2 == null ||prochainExercice.Item1 == string.Empty || prochainExercice.Item2 == string.Empty)
        {
            //Afficher un pop-up pas d'exercice disponible
            return RedirectToPage("/Dashboard");
        }

        return RedirectToPage("/Exercice", new { Titre = prochainExercice.Item1, LeconTitre = prochainExercice.Item2 });
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
