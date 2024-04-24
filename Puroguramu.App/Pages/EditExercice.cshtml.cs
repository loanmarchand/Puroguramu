using System.ComponentModel.DataAnnotations;
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
        Input = new InputModel { Titre = exercice.Titre, Enonce = exercice.Enonce, Modele = exercice.Modele, Solution = exercice.Solution, Difficulte = exercice.Difficulte};
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var exercice = _exercisesRepository.GetExercise(LeconTitre, ExerciceTitre);
        var tempTitre = exercice.Titre;
        exercice.Titre = Input.Titre;
        exercice.Enonce = Input.Enonce;
        exercice.Difficulte = Input.Difficulte;
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
            var res = _exercisesRepository.UpdateExercise(exercice,tempTitre, LeconTitre);
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
        [Required(ErrorMessage = "Le champ titre est requis")]
        [RegularExpression(@"^(?!.*<|>).*.{5,}$", ErrorMessage = "Le titre doit contenir au moins 5 caractères et ne doit pas inclure les symboles <>.")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "Le champ enoncé est requis")]
        [RegularExpression(@"^(?!.*<|>).*.{5,}$", ErrorMessage = "L'énoncé doit contenir au moins 5 caractères et ne doit pas inclure les symboles <>.")]
        public string Enonce { get; set; }

        [Required(ErrorMessage = "Le champ difficulte est requis")]
        [RegularExpression(@"^(?!.*<|>).*.{5,}$", ErrorMessage = "L'énoncé doit contenir au moins 5 caractères et ne doit pas inclure les symboles <>.")]
        public string Difficulte { get; set; }

        [Required(ErrorMessage = "Le champ modele est requis")]
        public string Modele { get; set; }

        [Required(ErrorMessage = "Le champ solution est requis")]
        public string Solution { get; set; }
    }
}
