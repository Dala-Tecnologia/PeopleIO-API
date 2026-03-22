using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PeopleIO.API.Endpoints;
using PeopleIO.Application;
using PeopleIO.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var origins = "allowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(origins, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173","https://red-flower-08bf62a0f.3.azurestaticapps.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var kvUrl = builder.Configuration["KeyVault:Url"];
if (!string.IsNullOrEmpty(kvUrl))
{
    builder.Configuration.AddAzureKeyVault(new Uri(kvUrl), new DefaultAzureCredential());
}

// DEBUG TEMPORÁRIO - remover após confirmar os valores
Console.WriteLine($"[DEBUG] KeyVault URL: '{builder.Configuration["KeyVault:Url"]}'");
Console.WriteLine($"[DEBUG] AzureAd:Audience: '{builder.Configuration["AzureAd:Audience"]}'");
Console.WriteLine($"[DEBUG] AzureAd:TenantId: '{builder.Configuration["AzureAd:TenantId"]}'");
Console.WriteLine($"[DEBUG] DI Endpoint: '{builder.Configuration["AzureDocumentIntelligence:Endpoint"]}'");
Console.WriteLine($"[DEBUG] DI ApiKey: '{builder.Configuration["AzureDocumentIntelligence:ApiKey"]}'");
Console.WriteLine($"[DEBUG] ConnectionString: '{(builder.Configuration["ConnectionStrings:PostgreSQL"] != null ? "OK (não nulo)" : "NULO")}'");

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddOpenApi();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://peopleioauth.ciamlogin.com/d86016d0-ba89-4553-a9ce-80757be65a93/v2.0"; 
        
        var audience = builder.Configuration["AzureAd:Audience"]!;
        options.Audience = audience;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudiences = new[] { audience, $"api://{audience}" },
            ValidIssuers = new[] 
            { 
                "https://peopleioauth.ciamlogin.com/d86016d0-ba89-4553-a9ce-80757be65a93/v2.0",
                "https://d86016d0-ba89-4553-a9ce-80757be65a93.ciamlogin.com/d86016d0-ba89-4553-a9ce-80757be65a93/v2.0"
            }
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError("Authentication failed: {Message}", context.Exception.Message);

                if (context.Exception is SecurityTokenInvalidIssuerException issuerEx)
                {
                    logger.LogError("Issuer validation failed. Token Issuer: '{Issuer}'. Expected: {Expected}", 
                        issuerEx.InvalidIssuer, 
                        string.Join(", ", options.TokenValidationParameters.ValidIssuers));
                }

                if (context.Exception is SecurityTokenInvalidAudienceException audEx)
                {
                    logger.LogError("Audience validation failed. Token Audience: '{Audience}'. Expected: {Expected}", 
                        audEx.InvalidAudience, 
                        string.Join(", ", options.TokenValidationParameters.ValidAudiences));
                }

                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                var audience = context.Principal?.FindFirst("aud")?.Value;
                
                logger.LogInformation("Token validated successfully. User: {User}, Issuer: {Issuer}, Audience: {Audience}", 
                    context.Principal?.Identity?.Name,
                    context.SecurityToken.Issuer,
                    audience);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("OnChallenge - 401 Triggered. Error: {Error}, Description: {ErrorDescription}", context.Error, context.ErrorDescription);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors(origins);

app.UseAuthentication();
app.UseAuthorization();

app.MapCandidatoEndpoints();
app.MapExperienciaEndpoints();
app.MapDocumentoEndpoints();

app.Run();