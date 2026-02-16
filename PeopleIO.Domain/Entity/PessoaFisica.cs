using System;
using PeopleIO.Domain.Entity;
using PeopleIO.Domain.Enums;

public abstract class PessoaFisica : BaseEntity
{
    public string Nome { get; set; } = default!;
    public string? NomeSocial { get; set; }
    public string CPF { get; set; } = default!;
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; } = default!;
    public string Telefone { get; set; } = default!;
    public Endereco Endereco { get; set; } = default!;
    
    public string? IdentidadeNumero { get; set; } = default!;
    public string? IdentidadeOrgaoEmissor { get; set; } = default!;
    public string? IdentidadeUF { get; set; } = default!;
    public DateTime? IdentidadeDataEmissao { get; set; }

    public string CTPSNumero { get; set; } = default!;
    public string CTPSSerie { get; set; } = default!;
    public DateTime CTPSDataEmissao { get; set; }
    public string CTPSUF { get; set; } = default!;

    public string? TituloEleitorNumero { get; set; } 
    public string? TituloUF { get; set; }
    public string? TituloZona { get; set; } 
    public string? TituloSecao { get; set; } 

    public string? CNHNumero { get; set; } 
    public string? CNHUF { get; set; }
    public DateTime? CNHDataVencimento { get; set; }
    public string? CNHOrgaoEmissor { get; set; } 
    public string? CNHTipo { get; set; }
    
    public CorRaca CorRaca { get; set; } = default!;
    public Sexo Sexo { get; set; } = default!;
    public string Escolaridade { get; set; } = default!;
    public string EstadoCivil { get; set; } = default!;
    public string Naturalidade { get; set; } = default!;
    public string Nacionalidade { get; set; } = default!;
}