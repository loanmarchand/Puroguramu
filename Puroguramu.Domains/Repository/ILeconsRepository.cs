namespace Puroguramu.Domains.Repository;

public interface ILeconsRepository
{
    IList<Lecon> GetLecons();

    Lecon GetLecon(string idLecons, string userId);

    Exercise GetExercice(string? leconTitre, string? titreExo);
    string GetExerciceId(string leconTitre, string titre);
    Task CreateLecon(string titreCours, string inputTitre);

    IEnumerable<Lecon> GetLeconsForCours(string titreCours, string userId);

}
