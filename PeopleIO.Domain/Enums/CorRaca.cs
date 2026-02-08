using System.ComponentModel.DataAnnotations;

namespace PeopleIO.Domain.Enum;

public enum CorRaca
{
    [Display(Name = "Branca")]
    Branca,
    [Display(Name = "Preta")]
    Preta,
    [Display(Name = "Parda")]
    Parda,
    [Display(Name = "Amarela")]
    Amarela,
    [Display(Name = "Indigena")]
    Indigena,
    [Display(Name = "Outro")]
    Outro,
    [Display(Name = "Não Informado")]
    NãoInformado

}