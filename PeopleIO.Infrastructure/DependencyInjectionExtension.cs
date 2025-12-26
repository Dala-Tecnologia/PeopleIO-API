using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeopleIO.Domain.Contract;
using PeopleIO.Infrastructure.Context;
using PeopleIO.Infrastructure.Repository;

namespace PeopleIO.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddRepository(services);
    }

    private static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
        
    }
    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["PEOPLEIO-CONN-STRING-PGSQL"];
        services.AddDbContext<PeopleIoContext>(opt => opt.UseNpgsql(connectionString));
    }
}