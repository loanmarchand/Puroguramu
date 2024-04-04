namespace Puroguramu.Domains;

public class Cour
{
    public string Titre { get; set; }
    public string ImageUrl { get; set; }
    public IList<Lecon> Lecons { get; set; }
}
