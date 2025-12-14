using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PeopleIO.Infrastructure.Context;

public class PeopleIOContextFactory : IDesignTimeDbContextFactory<PeopleIoContext>
{
    public PeopleIoContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PeopleIO.API"))
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSQL");

        var optionsBuilder = new DbContextOptionsBuilder<PeopleIoContext>();
        optionsBuilder.UseNpgsql(connectionString, o => o.MigrationsAssembly(typeof(PeopleIOContextFactory).Assembly.FullName));

        return new PeopleIoContext(optionsBuilder.Options);
    }
}
