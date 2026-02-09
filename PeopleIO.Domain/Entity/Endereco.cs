namespace PeopleIO.Domain.Entity;

public class Endereco
{
    public string Rua { get; set; } = default!;
    public string Numero { get; set; } = default!;
    public string Bairro { get; set; } = default!;
    public string Cidade { get; set; } = default!;
    public string Estado { get; set; } = default!;
    public string CEP { get; set; } = default!;
}
