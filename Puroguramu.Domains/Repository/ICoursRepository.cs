namespace Puroguramu.Domains.Repository;

public interface ICoursRepository
{
    IList<Cour> GetCours();
    IEnumerable<Lecon> GetLeconsForCours(string nameCours, string userId);
}
