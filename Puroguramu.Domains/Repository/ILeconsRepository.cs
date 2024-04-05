namespace Puroguramu.Domains.Repository;

public interface ILeconsRepository
{
    IList<Lecon> GetLecons();

    Lecon GetLecon(string idLecons, string userId);

    Exercise GetExercice(string? leconTitre, string? titreExo);
    string GetExerciceId(string leconTitre, string titre);
}
