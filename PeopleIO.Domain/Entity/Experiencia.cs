using System.Text.Json.Serialization;
using PeopleIO.Domain.Enums;

namespace PeopleIO.Domain.Entity;

public class Experiencia : BaseEntity
{
    public string NomeEmpresa { get; set; } = string.Empty;
    public TipoContrato TipoContrato { get; set; }
    public string Funcao { get; set; } = string.Empty;
    public bool TrabalhandoAtualmente { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataTermino { get; set; }
    public string Local { get; set; } = string.Empty;
    public TipoAtuacao Atuacao { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public Guid CandidatoId { get; set; }
    [JsonIgnore]
    public Candidato? Candidato { get; set; }
}