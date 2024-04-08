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
        return _template.Replace("// modele", Modele).Replace("// code-insertion-point", code);
    }
}
