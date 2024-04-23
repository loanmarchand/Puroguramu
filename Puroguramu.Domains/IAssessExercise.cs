namespace Puroguramu.Domains;

public interface IAssessExercise
{
    Task<ExerciseResult> Assess(string exerciseId, string proposal);

    Task<ExerciseResult> StubForExercise(string exerciseId);

    Task<ExerciseResult> Assess(Exercise exercise, string proposal);
}
