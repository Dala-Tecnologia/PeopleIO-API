using PeopleIO.Domain.Enums;

namespace PeopleIO.Domain.Entity;

public class Experiencia
{
    public int Id { get; set; }
    public string NomeEmpresa { get; set; } = string.Empty;
    public TipoContrato TipoContrato { get; set; }
    public string Funcao { get; set; } = string.Empty;
    public bool TrabalhandoAtualmente { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataTermino { get; set; }
    public string Local { get; set; } = string.Empty;
    public TipoAtuacao Atuacao { get; set; }
    public string Descricao { get; set; } = string.Empty;
}