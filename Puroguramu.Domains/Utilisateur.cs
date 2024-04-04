namespace Puroguramu.Domains;

public class Utilisateur
{
    public string? Matricule { get; set; }

    public string? Nom { get; set; }

    public string? Prenom { get; set; }

    public string? Groupe { get; set; }

    public Role? Role { get; set; }
}
