namespace Puroguramu.Domains;

public class Lecon
{
    public string Titre { get; set; }

    public string Description { get; set; }

    public bool estVisible { get; set; }

    public IList<Exercise> ExercicesList { get; set; }
    public int? ExercicesFait { get; set; }
    public int? ExercicesTotal { get; set; }
}
