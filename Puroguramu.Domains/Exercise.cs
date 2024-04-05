namespace Puroguramu.Domains;

public class Exercise
{
    public string Titre { get; set; }

    public string Enonce { get; set; }

    public string Modele { get; set; }

    public string Solution { get; set; }

    public bool EstVisible { get; set; }

    public string Difficulte { get; set; }

    public Status etat;

    private readonly string _template = @"// code-insertion-point

// modele
";


    public string Stub => @"public class Exercice
{
  // Tapez votre code ici
}
";

    public string InjectIntoTemplate(string code)
    {
        Console.WriteLine(Modele);
        return _template.Replace("// modele", PreprocessModel(Modele)).Replace("// code-insertion-point", code);
    }

    public string PreprocessModel(string modelFromDb)
    {
        // Échapper les guillemets doubles
        string escapedModel = modelFromDb.Replace("\"", "\\\"");

        // Doubler les accolades pour string.Format
        escapedModel = escapedModel.Replace("{", "{{").Replace("}", "}}");

        return escapedModel;
    }

}
