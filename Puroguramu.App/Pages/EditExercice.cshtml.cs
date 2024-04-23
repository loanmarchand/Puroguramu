using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Repository;

namespace Puroguramu.App.Pages;

[ValidateAntiForgeryToken]
[Authorize(Policy = "IsTeacher")]
public class EditExercice : PageModel
{
    private readonly IExercisesRepository _exercisesRepository;
    private readonly IAssessExercise _assessor;

    public EditExercice(IExercisesRepository exercisesRepository, IAssessExercise assessor)
    {
        _exercisesRepository = exercisesRepository;
        _assessor = assessor;
    }

    public string ReturnUrl { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    [BindProperty(SupportsGet = true)] public string LeconTitre { get; set; }

    [BindProperty(SupportsGet = true)] public string ExerciceTitre { get; set; }

    public void OnGet()
    {
        var exercice = _exercisesRepository.GetExercise(LeconTitre, ExerciceTitre);
        Input = new InputModel { Titre = exercice.Titre, Enonce = exercice.Enonce, Modele = exercice.Modele, Solution = exercice.Solution };
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var exercice = _exercisesRepository.GetExercise(LeconTitre, ExerciceTitre);
        exercice.Titre = Input.Titre;
        exercice.Enonce = Input.Enonce;
        exercice.Modele = Input.Modele;
        exercice.Solution = Input.Solution;

        var test = _assessor.Assess(exercice, Input.Solution).Result;
        var success = true;
        foreach (var s in test.TestResults)
        {
            //Afficher les erreurs
            if (s.Status == TestStatus.Passed)
            {
                continue;
            }

            ModelState.AddModelError("Input.Solution", s.ErrorMessage);
            success = false;
        }

        if (success)
        {
            var res = _exercisesRepository.UpdateExercise(exercice, LeconTitre);
            if (res.Result)
            {
                return RedirectToPage("/EditLecon", new { LeconTitre });
            }

            ModelState.AddModelError("Input.Titre", "Le titre de l'exercice existe déjà");
        }

        return Page();
    }

    public class InputModel
    {
        public string Titre { get; set; }

        public string Enonce { get; set; }

        public string Modele { get; set; }

        public string Solution { get; set; }
    }
}
