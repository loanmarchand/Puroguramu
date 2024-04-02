using Microsoft.AspNetCore.Identity;

namespace Puroguramu.Infrastructures.dto;

public class Utilisateurs : IdentityUser
{
    public string? Matricule { get; set; }

    public string? Nom { get; set; }

    public string? Prenom { get; set; }

    public string? Groupe { get; set; }

    public Role? Role { get; set; }
}
