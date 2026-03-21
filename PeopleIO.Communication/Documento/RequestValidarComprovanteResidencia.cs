namespace PeopleIO.Communication;

public record RequestValidarComprovanteResidencia(
    string BlobUrl,
    string Nome,
    string Rua,
    string Cidade,
    string CEP
);
