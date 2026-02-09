using PeopleIO.Domain.Entity;
using PeopleIO.Domain.Enums;

namespace PeopleIO.Communication;

public record ExperienciaDTO(
    Guid Id,
    string NomeEmpresa,
    TipoContrato TipoContrato,
    string Funcao,
    bool TrabalhandoAtualmente,
    DateTime DataInicio,
    DateTime? DataTermino,
    string Local,
    TipoAtuacao Atuacao,
    string Descricao,
    Guid CandidatoId)
{
    public Experiencia ToEntity() => new()
    {
        Id = Id,
        NomeEmpresa = NomeEmpresa,
        TipoContrato = TipoContrato,
        Funcao = Funcao,
        TrabalhandoAtualmente = TrabalhandoAtualmente,
        DataInicio = DataInicio,
        DataTermino = DataTermino,
        Local = Local,
        Atuacao = Atuacao,
        Descricao = Descricao,
        CandidatoId = CandidatoId

    };
};

public static class ExperienciaExtensions
{
    public static ExperienciaDTO ToDto(this Experiencia entity)
    {
        return new ExperienciaDTO(
            entity.Id,
            entity.NomeEmpresa,
            entity.TipoContrato,
            entity.Funcao,
            entity.TrabalhandoAtualmente,
            entity.DataInicio,
            entity.DataTermino,
            entity.Local,
            entity.Atuacao,
            entity.Descricao,
            entity.CandidatoId
            );
    }

    public static void UpdateFrom(this Experiencia entity, ExperienciaDTO request)
    {
        entity.Id = request.Id;
        entity.NomeEmpresa = request.NomeEmpresa;
        entity.TipoContrato = request.TipoContrato;
        entity.Funcao = request.Funcao;
        entity.TrabalhandoAtualmente = request.TrabalhandoAtualmente;
        entity.DataInicio = request.DataInicio;
        entity.DataTermino = request.DataTermino;
        entity.Local = request.Local;
        entity.Atuacao = request.Atuacao;
        entity.Descricao = request.Descricao;
        entity.CandidatoId = request.CandidatoId;
    
    }
}