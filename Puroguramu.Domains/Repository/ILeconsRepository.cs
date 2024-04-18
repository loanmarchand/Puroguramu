namespace Puroguramu.Domains.Repository;

public interface ILeconsRepository
{
    IList<Lecon> GetLecons();

    Lecon? GetLeconWithStatuts(string idLecons, string userId);

    Exercise GetExercice(string? leconTitre, string? titreExo);

    string GetExerciceId(string leconTitre, string titre);

    Task CreateLecon(string titreCours, string inputTitre);

    IEnumerable<Lecon> GetLeconsForCours(string titreCours, string userId);

    Task<(string, string)> GetNextExerciceAsync(string titreCours, string userId);

    Task<(string, string)> GetActualExercicesAsync(string titreCours, string userId);

    Lecon GetLecon(string leconTitre);

    void UpdateLecon(string leconTitre, string inputTitre, string inputDescription);

    Task ToggleVisibilityLecon(string leconTitre);

    Task DeleteLecon(string leconTitre);
}
