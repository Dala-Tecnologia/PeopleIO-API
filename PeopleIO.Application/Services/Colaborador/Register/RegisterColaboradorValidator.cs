using FluentValidation;
using PeopleIO.Communication;
using PeopleIO.Exceptions;

namespace PeopleIO.Application.Services.Colaborador.Register;

public class RegisterColaboradorValidator : AbstractValidator<RequestRegisterColaborador>
{
    public RegisterColaboradorValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.NAME_REQUIRED);
        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Length(11).WithMessage("CPF deve conter exatamente 11 caracteres")
            .Matches("^[0-9]+$").WithMessage("CPF deve conter apenas números");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido");
        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Telefone é obrigatório");
        RuleFor(x => x.Endereco.Rua)
            .NotEmpty().WithMessage("A Rua é obrigatório.");
        RuleFor(x => x.Endereco.Numero)
            .NotEmpty().WithMessage("O número é obrigatório.");
        RuleFor(x => x.Endereco.Bairro)
            .NotEmpty().WithMessage("Bairro é obrigatório.");
        RuleFor(x => x.Endereco.Cidade)
            .NotEmpty().WithMessage("A Cidade é obrigatório.");
        RuleFor(x => x.Endereco.Estado)
            .NotEmpty().WithMessage("Estado é obrigatório.");
        RuleFor(x => x.Endereco.CEP)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Matches(@"^\d{8}$").WithMessage("CEP deve conter exatamente 8 dígitos numéricos");
    }
    
    private async Task<bool> ValidarCepExiste(string cep, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return false;

        using var client = new HttpClient();

        try
        {
            var response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/", cancellationToken);

            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            // O ViaCEP retorna {"erro": true} quando o CEP não existe
            return !json.Contains("\"erro\": true");
        }
        catch
        {
            // Se der erro de rede ou timeout, considerar inválido
            return false;
        }
    }
}