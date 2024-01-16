namespace Porugumaru.App.Core.Dummies;

public class DummyExercicesRepository : IExercicesRepository
{
    public Exercice GetExercice(Guid exerciceId)
        => new Exercice();
}