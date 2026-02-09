using System.ComponentModel.DataAnnotations;

namespace PeopleIO.Domain.Enums;

public enum TipoAtuacao
{
    [Display(Name = "Presencial")]
    Presencial,
    [Display(Name = "Remoto")]
    Remoto,
    [Display(Name = "Hibrido")]
    Hibrido
}