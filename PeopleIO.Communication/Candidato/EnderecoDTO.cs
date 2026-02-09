namespace PeopleIO.Communication;

public record EnderecoDTO(
    string Rua,
    string Numero,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP
);