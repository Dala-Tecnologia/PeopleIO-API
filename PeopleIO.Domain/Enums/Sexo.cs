using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PeopleIO.Domain.Enum;

public enum Sexo
{
    [Display(Name = "Masculino")]
    Masculino,
    [Display(Name = "Feminino")]
    Feminino,
    [Display(Name = "Prefiro não informar")]
    PrefiroNaoInformar,
    [Display(Name = "Outro")]
    Outro
}