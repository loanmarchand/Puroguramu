namespace Puroguramu.Domains.Repository;

public interface ILeconsRepository
{
    IList<Lecon> GetLecons();

    Lecon? GetLeconWithStatuts(string idLecons, string userId);

    Exercise GetExercice(string? leconTitre, string? titreExo);

    string GetExerciceId(string leconTitre, string titre);

    Task<bool> CreateLecon(string titreLecon);

    IEnumerable<Lecon> GetLeconsForCours(string userId);

    Task<(string, string)> GetNextExerciceAsync(string userId);

    Task<(string, string)> GetActualExercicesAsync(string userId);

    Lecon GetLecon(string leconTitre);

    Task<bool> UpdateLecon(string leconTitre, string inputTitre, string inputDescription);

    Task ToggleVisibilityLecon(string leconTitre);

    Task DeleteLecon(string leconTitre);
    int? GetExercicesFait(string leconTitre);
    Task MoveLecon(string leconTitre, string direction);
}
