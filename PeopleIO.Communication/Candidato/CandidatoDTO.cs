using PeopleIO.Domain.Entity;
using PeopleIO.Domain.Enums;

namespace PeopleIO.Communication;

public record CandidatoDTO(
    string Nome,
    string? NomeSocial,
    string CPF,
    DateTime DataNascimento,
    string Email,
    string Telefone,
    Endereco Endereco,
    string IdentidadeNumero,
    string IdentidadeOrgaoEmissor,
    string IdentidadeUF,
    DateTime IdentidadeDataEmissao,
    string CTPSNumero,
    string CTPSSerie,
    DateTime CTPSDataEmissao,
    string CTPSUF,
    string? TituloEleitorNumero,
    string? TituloUF,
    string? TituloZona,
    string? TituloSecao,
    string? CNHNumero,
    string? CNHUF,
    DateTime? CNHDataVencimento,
    string? CNHOrgaoEmissor,
    string? CNHTipo,
    CorRaca CorRaca,
    Sexo Sexo,
    string Escolaridade,
    string EstadoCivil,
    string Naturalidade,
    string Nacionalidade,
    string? Cargo,
    string? Departamento,
    DateTime DataAdmissao,
    DateTime? DataDemissao,
    Documento? ArquivoRG,
    Documento? ArquivoCNH,
    Documento? ArquivoCPF,
    Documento? ArquivoComprovanteResidencia,
    Documento? Curriculo,
    Documento? FotoUrl,
    List<Experiencia>? experiencias,
    bool Ativo
)
{
    public Candidato ToEntity() => new()
    {
        Nome = Nome,
        NomeSocial = NomeSocial,
        CPF = CPF,
        DataNascimento = DataNascimento,
        Email = Email,
        Telefone = Telefone,
        Endereco = Endereco,
        IdentidadeNumero = IdentidadeNumero,
        IdentidadeOrgaoEmissor = IdentidadeOrgaoEmissor,
        IdentidadeUF = IdentidadeUF,
        IdentidadeDataEmissao = IdentidadeDataEmissao,
        CTPSNumero = CTPSNumero,
        CTPSSerie = CTPSSerie,
        CTPSDataEmissao = CTPSDataEmissao,
        CTPSUF = CTPSUF,
        TituloEleitorNumero = TituloEleitorNumero,
        TituloUF = TituloUF,
        TituloZona = TituloZona,
        TituloSecao = TituloSecao,
        CNHNumero = CNHNumero,
        CNHUF = CNHUF,
        CNHDataVencimento = CNHDataVencimento,
        CNHOrgaoEmissor = CNHOrgaoEmissor,
        CNHTipo = CNHTipo,
        CorRaca = CorRaca,
        Sexo = Sexo,
        Escolaridade = Escolaridade,
        EstadoCivil = EstadoCivil,
        Naturalidade = Naturalidade,
        Nacionalidade = Nacionalidade,
        Cargo = Cargo,
        Departamento = Departamento,
        DataAdmissao = DataAdmissao,
        DataDemissao = DataDemissao,
        ArquivoRG = ArquivoRG,
        ArquivoCNH = ArquivoCNH,
        ArquivoCPF = ArquivoCPF,
        ArquivoComprovanteResidencia = ArquivoComprovanteResidencia,
        Curriculo = Curriculo,
        FotoUrl = FotoUrl,
        Experiencias = experiencias,
        Ativo = Ativo
    };
};

public static class CandidatoExtensions
{
    public static CandidatoDTO ToDto(this Candidato entity)
    {
        return new CandidatoDTO(
            entity.Nome,
            entity.NomeSocial,
            entity.CPF,
            entity.DataNascimento,
            entity.Email,
            entity.Telefone,
            entity.Endereco,
            entity.IdentidadeNumero,
            entity.IdentidadeOrgaoEmissor,
            entity.IdentidadeUF,
            entity.IdentidadeDataEmissao,
            entity.CTPSNumero,
            entity.CTPSSerie,
            entity.CTPSDataEmissao,
            entity.CTPSUF,
            entity.TituloEleitorNumero,
            entity.TituloUF,
            entity.TituloZona,
            entity.TituloSecao,
            entity.CNHNumero,
            entity.CNHUF,
            entity.CNHDataVencimento,
            entity.CNHOrgaoEmissor,
            entity.CNHTipo,
            entity.CorRaca,
            entity.Sexo,
            entity.Escolaridade,
            entity.EstadoCivil,
            entity.Naturalidade,
            entity.Nacionalidade,
            entity.Cargo,
            entity.Departamento,
            entity.DataAdmissao ?? new DateTime(),
            entity.DataDemissao,
            entity.ArquivoRG,
            entity.ArquivoCNH,
            entity.ArquivoCPF,
            entity.ArquivoComprovanteResidencia,
            entity.Curriculo,
            entity.FotoUrl,
            entity.Experiencias,
            entity.Ativo
        );
    }

    public static void UpdateFrom(this Candidato entity, CandidatoDTO request)
    {
        entity.Nome = request.Nome;
        entity.NomeSocial = request.NomeSocial;
        entity.CPF = request.CPF;
        entity.DataNascimento = request.DataNascimento;
        entity.Email = request.Email;
        entity.Telefone = request.Telefone;
        entity.Endereco = request.Endereco;
        entity.IdentidadeNumero = request.IdentidadeNumero;
        entity.IdentidadeOrgaoEmissor = request.IdentidadeOrgaoEmissor;
        entity.IdentidadeUF = request.IdentidadeUF;
        entity.IdentidadeDataEmissao = request.IdentidadeDataEmissao;
        entity.CTPSNumero = request.CTPSNumero;
        entity.CTPSSerie = request.CTPSSerie;
        entity.CTPSDataEmissao = request.CTPSDataEmissao;
        entity.CTPSUF = request.CTPSUF;
        entity.TituloEleitorNumero = request.TituloEleitorNumero;
        entity.TituloUF = request.TituloUF;
        entity.TituloZona = request.TituloZona;
        entity.TituloSecao = request.TituloSecao;
        entity.CNHNumero = request.CNHNumero;
        entity.CNHUF = request.CNHUF;
        entity.CNHDataVencimento = request.CNHDataVencimento;
        entity.CNHOrgaoEmissor = request.CNHOrgaoEmissor;
        entity.CNHTipo = request.CNHTipo;
        entity.CorRaca = request.CorRaca;
        entity.Sexo = request.Sexo;
        entity.Escolaridade = request.Escolaridade;
        entity.EstadoCivil = request.EstadoCivil;
        entity.Naturalidade = request.Naturalidade;
        entity.Nacionalidade = request.Nacionalidade;
        entity.Cargo = request.Cargo;
        entity.Departamento = request.Departamento;
        entity.DataAdmissao = request.DataAdmissao;
        entity.DataDemissao = request.DataDemissao;
        entity.ArquivoRG = request.ArquivoRG;
        entity.ArquivoCNH = request.ArquivoCNH;
        entity.ArquivoCPF = request.ArquivoCPF;
        entity.ArquivoComprovanteResidencia = request.ArquivoComprovanteResidencia;
        entity.Curriculo = request.Curriculo;
        entity.FotoUrl = request.FotoUrl;
        entity.Experiencias = request.experiencias;
        entity.Ativo = request.Ativo;
    }
}