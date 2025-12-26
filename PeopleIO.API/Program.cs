using System.IdentityModel.Tokens.Jwt;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PeapleIO.API.Endpoints;
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

builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["KeyVault:Url"]!),
    new DefaultAzureCredential());

builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddOpenApi();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://peopleioauth.ciamlogin.com/d86016d0-ba89-4553-a9ce-80757be65a93/v2.0"; 
        options.Audience = builder.Configuration["AzureAd:Audience"]!; 
    });

builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseAuthentication(); 
app.UseAuthorization(); 

app.UseHttpsRedirection();
app.UseCors(origins);

app.MapColaboradorEndpoints();

app.Run();