using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace PeopleIO.Application.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(IConfiguration config)
    {
        var connectionString = config["BlobStorageConnectionString"]; //"DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net"
        var containerName = config["BlobStorageContainerName"]; //"documentos"

        var serviceClient = new BlobServiceClient(connectionString);
        _containerClient = serviceClient.GetBlobContainerClient(containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadAsync(IFormFile file, string fileName)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        using var stream = file.OpenReadStream();
        
        var blobUploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
        };

        await blobClient.UploadAsync(stream, options: blobUploadOptions);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> GetBlobStreamAsync(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        return await blobClient.OpenReadAsync();
    }
}