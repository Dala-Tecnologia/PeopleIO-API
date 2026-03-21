# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the solution
dotnet build

# Build in release mode
dotnet build --configuration Release

# Run the API
dotnet run --project PeopleIO.API

# Run all tests
dotnet test

# Run a specific test project
dotnet test PeopleIO.Tests

# Add a new EF Core migration (run from repo root)
dotnet ef migrations add <MigrationName> --project PeopleIO.Infrastructure --startup-project PeopleIO.API

# Apply migrations
dotnet ef database update --project PeopleIO.Infrastructure --startup-project PeopleIO.API
```

## Architecture

This is a .NET 10 ASP.NET Core Web API following Clean Architecture, organized into six projects:

- **PeopleIO.API** — Entry point. Wires up services via `Program.cs`. Exposes endpoints through Minimal API extension classes (`CandidatoEndpoints`, `ExperienciaEndpoints`) and uses `AddInfraestructure()` / `AddApplication()` extension methods to register dependencies.
- **PeopleIO.Application** — Business logic. Services follow an interface-per-operation pattern under `Services/<Entity>/<Operation>/`. Uses `Result<T>` (in `Results/Result.cs`) to return success/failure without exceptions. FluentValidation validators live alongside their service classes. Mapster is used for object mapping, configured in `Mappers/MapsterConfig.cs`.
- **PeopleIO.Domain** — Core entities (`Candidato`, `PessoaFisica`, `Experiencia`, `Endereco`, `Documento`) and repository contracts (`ICRUDRepository<T>`, `ICandidatoRepository`). No external dependencies.
- **PeopleIO.Infrastructure** — EF Core with PostgreSQL (Npgsql). `PeopleIoContext` maps owned entities (Endereco, Documento variants) inline. Connection string is read from Azure Key Vault via the key `PEOPLEIO-CONN-STRING-PGSQL`.
- **PeopleIO.Communication** — Shared DTOs and request/response models used across layers.
- **PeopleIO.Exceptions** — Shared exception resource messages.
- **PeopleIO.Tests** — xUnit tests using Moq and FluentAssertions.

### Key domain model

`Candidato` extends `PessoaFisica` (which extends `BaseEntity`) and includes Brazilian identity document fields (RG, CNH, CTPS, CPF, Título de Eleitor), `Endereco` (owned), and multiple `Documento` owned entities for file uploads (RG, CNH, CPF, comprovante de residência, currículo, foto).

### Azure integrations

- **Authentication** — Azure AD CIAM (`peopleioauth.ciamlogin.com`), JWT Bearer, validated in `Program.cs`. Audience is read from Key Vault via `AzureAd:Audience`.
- **Key Vault** — All secrets (DB connection string, AD audience, blob storage, document intelligence API key) are loaded from Azure Key Vault. The vault URL comes from `KeyVault:Url` in config.
- **Blob Storage** — `BlobStorageService` (singleton) handles file uploads/downloads from Azure Blob Storage.
- **Document Intelligence** — `DocumentValidationService` uses `Azure.AI.DocumentIntelligence` (`prebuilt-idDocument` model) to validate RG, CNH, and proof-of-residence documents during candidate registration.

### Adding a new operation

1. Define the service interface in `PeopleIO.Application/Services/<Entity>/<Operation>/I<Name>Service.cs`
2. Implement the service in the same folder, returning `Result<T>` for operations that can fail
3. Register the scoped service in `PeopleIO.Application/DependencyInjectionExtension.cs`
4. Map the endpoint in the relevant `PeopleIO.API/Endpoints/<Entity>Endpoints.cs`

### CI/CD

Pushes to `main` trigger the GitHub Actions workflow (`.github/workflows/main_peopleio-api-dev.yml`), which builds in Release mode and deploys to the Azure App Service `peopleio-api-dev`.

### API documentation

In development, Scalar API reference is available at `/scalar`. OpenAPI spec is served at `/openapi`.
