namespace PeopleIO.Communication;

public record EnderecoRequest(
    string Rua,
    string Numero,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP
);