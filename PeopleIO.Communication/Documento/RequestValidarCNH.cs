namespace PeopleIO.Communication;

public record RequestValidarCNH(
    string BlobUrl,
    string CNHNumero,
    string CPF,
    DateTime DataNascimento,
    DateTime? CNHDataVencimento
);
