using Microsoft.AspNetCore.Http;

namespace PeopleIO.Application.Services;

public interface IBlobStorageService
{
    Task<string> UploadAsync(IFormFile file, string fileName);
}