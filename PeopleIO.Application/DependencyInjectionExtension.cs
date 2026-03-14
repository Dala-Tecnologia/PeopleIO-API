using Microsoft.Extensions.DependencyInjection;
using PeopleIO.Application.Mappers;
using PeopleIO.Application.Services;
using PeopleIO.Application.Services.Candidato.GetAll;
using PeopleIO.Application.Services.Candidato.GetById;
using PeopleIO.Application.Services.Candidato.Delete;
using PeopleIO.Application.Services.Candidato.Register;
using PeopleIO.Application.Services.Candidato.Update;
using PeopleIO.Application.Services.Experiencia.Delete;

namespace PeopleIO.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddServices(services);
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        MapsterConfig.RegisterMappings();
        services.AddSingleton<IBlobStorageService, BlobStorageService>();
        services.AddScoped<IDocumentValidationService, DocumentValidationService>();
        services.AddScoped<IRegisterCandidatoService, RegisterCandidatoService>();
        services.AddScoped<IGetAllCandidatosService, GetAllCandidatosService>();
        services.AddScoped<IGetCandidatoByIdService, GetCandidatoByIdService>();
        services.AddScoped<IRemoveCandidatoService, RemoveCandidatoService>();
        services.AddScoped<IUpdateCandidatoService, UpdateCandidatoService>();
        
        services.AddScoped<IRemoveExperienciaService, RemoveExperienciaService>();
    }
}
