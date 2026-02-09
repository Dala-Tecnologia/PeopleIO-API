using Mapster;
using PeopleIO.Communication;
using PeopleIO.Domain.Entity;

namespace PeopleIO.Application.Mappers;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<EnderecoDTO, Endereco>
            .NewConfig();
        
        TypeAdapterConfig<RequestRegisterCandidato, Candidato>
            .NewConfig()
            .Map(dest => dest.Endereco, src => src.Endereco.Adapt<Endereco>())
            .Map(dest => dest.Cargo, src => src.Cargo)
            .Map(dest => dest.Departamento, src => src.Departamento)
            .Map(dest => dest.DataAdmissao, src => src.DataAdmissao)
            .Map(dest => dest.Nome, src => src.Nome)
            .Map(dest => dest.NomeSocial, src => src.NomeSocial)
            .Map(dest => dest.CPF, src => src.CPF)
            .Map(dest => dest.DataNascimento, src => src.DataNascimento)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Telefone, src => src.Telefone)
            .Map(dest => dest.IdentidadeNumero, src => src.IdentidadeNumero)
            .Map(dest => dest.IdentidadeOrgaoEmissor, src => src.IdentidadeOrgaoEmissor)
            .Map(dest => dest.IdentidadeUF, src => src.IdentidadeUF)
            .Map(dest => dest.IdentidadeDataEmissao, src => src.IdentidadeDataEmissao)
            .Map(dest => dest.CTPSNumero, src => src.CTPSNumero)
            .Map(dest => dest.CTPSSerie, src => src.CTPSSerie)
            .Map(dest => dest.CTPSDataEmissao, src => src.CTPSDataEmissao)
            .Map(dest => dest.CTPSUF, src => src.CTPSUF)
            .Map(dest => dest.TituloEleitorNumero, src => src.TituloEleitorNumero)
            .Map(dest => dest.TituloUF, src => src.TituloUF)
            .Map(dest => dest.TituloZona, src => src.TituloZona)
            .Map(dest => dest.TituloSecao, src => src.TituloSecao)
            .Map(dest => dest.CNHNumero, src => src.CNHNumero)
            .Map(dest => dest.CNHUF, src => src.CNHUF)
            .Map(dest => dest.CNHDataVencimento, src => src.CNHDataVencimento)
            .Map(dest => dest.CNHOrgaoEmissor, src => src.CNHOrgaoEmissor)
            .Map(dest => dest.CNHTipo, src => src.CNHTipo)
            .Map(dest => dest.CorRaca, src => src.CorRaca)
            .Map(dest => dest.Sexo, src => src.Sexo)
            .Map(dest => dest.Escolaridade, src => src.Escolaridade)
            .Map(dest => dest.EstadoCivil, src => src.EstadoCivil)
            .Map(dest => dest.Naturalidade, src => src.Naturalidade)
            .Map(dest => dest.Nacionalidade, src => src.Nacionalidade);
    }

}