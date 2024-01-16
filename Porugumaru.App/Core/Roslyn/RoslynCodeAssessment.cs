using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Porugumaru.App.Core.Roslyn;

public class RoslynCodeAssessment : IAssessCode
{
    private readonly IExercicesRepository _exercicesRepository;

    public RoslynCodeAssessment(IExercicesRepository repository)
    {
        _exercicesRepository = repository;
    }
    
    public IAssessmentResult Assess(Guid exerciceId, string code)
    {
        var exercice = _exercicesRepository.GetExercice(exerciceId);
        var codeToRun = exercice.MergeTemplateWith(code);
        try
        {
            Task<ScriptState<string[]>> runTask = CSharpScript.RunAsync<string[]>(codeToRun,
                ScriptOptions.Default.WithImports("System")); 
            var result = runTask.Result;

            return ExerciceResult.Parse(code, result.Variables[0].Value as string[]);
        }
        catch (CompilationErrorException ex)
        {
            return new ExerciceResult(AssessmentStatus.Failed, code, ex.Diagnostics.Select(d => d.ToString()));
        }
    }
}