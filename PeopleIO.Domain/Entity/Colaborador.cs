namespace PeopleIO.Domain.Entity;

public class Colaborador : Pessoa
{
    public string? Cargo { get; set; }
    public string? Departamento { get; set; }
    public DateTime DataAdmissao { get; set; }
    public DateTime? DataDemissao { get; set; }
    public Documento? ArquivoRG { get; set; }
    public Documento? ArquivoCNH { get; set; }
    public Documento? ArquivoCPF { get; set; }
    public Documento? ArquivoComprovanteResidencia { get; set; }
    public Documento? Curriculo { get; set; }
    public bool Ativo { get; set; } = true;
    
}
