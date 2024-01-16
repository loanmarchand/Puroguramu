namespace Porugumaru.App.Core;

public interface IAssessmentResult
{
    AssessmentStatus Status { get; }
    string Proposal { get; }
    
    IEnumerable<string> Tests { get; }

    bool IsNone
        => Status == AssessmentStatus.None;
}

public enum AssessmentStatus
{
    Passed,
    Failed,
    None
}