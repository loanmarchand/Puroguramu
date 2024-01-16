namespace Porugumaru.App.Core;

public interface IExercicesRepository
{
    Exercice GetExercice(Guid exerciceId);
}