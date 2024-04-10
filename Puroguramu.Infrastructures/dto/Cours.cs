using System.ComponentModel.DataAnnotations;

namespace Puroguramu.Infrastructures.dto;

public class Cours
{
    [Key]
    public string IdCours { get; set; }

    public string Titre { get; set; }

    public string ImageUrl { get; set; }

    public IList<Lecons>? Lecons { get; set; }

}
