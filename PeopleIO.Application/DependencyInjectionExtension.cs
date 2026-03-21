using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeopleIO.Application.Mappers;
using PeopleIO.Application.Services;
using PeopleIO.Application.Services.Candidato.GetAll;
using PeopleIO.Application.Services.Candidato.GetById;
using PeopleIO.Application.Services.Candidato.Delete;
using PeopleIO.Application.Services.Candidato.Register;
using PeopleIO.Application.Services.Candidato.Update;
using PeopleIO.Application.Services.Experiencia.Delete;
using PeopleIO.Application.Services.Documento.ValidarRG;
using PeopleIO.Application.Services.Documento.ValidarCNH;
using PeopleIO.Application.Services.Documento.ValidarComprovante;

namespace PeopleIO.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddServices(services, configuration);
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        MapsterConfig.RegisterMappings();

        services.AddSingleton(_ =>
        {
            var endpoint = configuration["AzureDocumentIntelligence:Endpoint"]!;
            var apiKey = configuration["AzureDocumentIntelligence:ApiKey"]!;
            return new DocumentIntelligenceClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        });

        services.AddSingleton<IBlobStorageService, BlobStorageService>();
        services.AddSingleton<IDocumentValidationService, DocumentValidationService>();
        services.AddScoped<IRegisterCandidatoService, RegisterCandidatoService>();
        services.AddScoped<IGetAllCandidatosService, GetAllCandidatosService>();
        services.AddScoped<IGetCandidatoByIdService, GetCandidatoByIdService>();
        services.AddScoped<IRemoveCandidatoService, RemoveCandidatoService>();
        services.AddScoped<IUpdateCandidatoService, UpdateCandidatoService>();
        
        services.AddScoped<IRemoveExperienciaService, RemoveExperienciaService>();
        services.AddScoped<IValidarRGService, ValidarRGService>();
        services.AddScoped<IValidarCNHService, ValidarCNHService>();
        services.AddScoped<IValidarComprovanteService, ValidarComprovanteService>();
    }
}
