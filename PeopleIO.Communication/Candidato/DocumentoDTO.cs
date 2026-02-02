namespace PeopleIO.Communication;

public record DocumentoDTO(
    string? NomeArquivo, 
    string? Url,
    DateTime? DataUpload,
    string? TipoMime);