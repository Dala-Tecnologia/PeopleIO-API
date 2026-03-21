namespace PeopleIO.Communication;

public record RequestValidarRG(
    string BlobUrl,
    string IdentidadeNumero,
    string IdentidadeUF,
    DateTime? IdentidadeDataEmissao
);
