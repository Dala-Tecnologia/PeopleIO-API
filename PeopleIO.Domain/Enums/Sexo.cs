using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PeopleIO.Domain.Enums;

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