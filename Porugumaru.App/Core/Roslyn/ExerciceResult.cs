namespace Porugumaru.App.Core.Roslyn;

public record ExerciceResult(AssessmentStatus Status, string Proposal, IEnumerable<string>? Tests = null) : IAssessmentResult
{
    public static IAssessmentResult Parse(string proposal, IEnumerable<string> testsSuite)
    {
        IEnumerable<string> testsSuitList = testsSuite?.ToArray() ?? Array.Empty<string>();
        bool anyFailed = testsSuitList.Any(test => test.Contains("FAILED"));
        return anyFailed
            ? new ExerciceResult(AssessmentStatus.Failed, proposal, testsSuitList)
            : new ExerciceResult(AssessmentStatus.Passed, proposal, testsSuitList);
    }
}

