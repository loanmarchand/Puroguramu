namespace Porugumaru.App.Core;

public interface IAssessCode
{
    IAssessmentResult Assess(Guid exerciceId, string code);
}
