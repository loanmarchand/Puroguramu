namespace Puroguramu.Domains.Repository;

public interface ILeconsRepository
{
    IList<Lecon> GetLecons();

    Lecon GetLecon(string idLecons);

    void AddLecon(Lecon lecon);

    void UpdateLecon(Lecon lecon);

    void DeleteLecon(Lecon lecon);

    IEnumerable<Exercise>? GetExercicesForLecon(string leconTitre);
    Exercise GetExercice(string? leconTitre, string? titreExo);
    string GetExerciceId(string leconTitre, string titre);
}
