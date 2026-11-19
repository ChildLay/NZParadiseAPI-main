using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class AzureBlobService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public AzureBlobService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureBlob:ConnectionString"];
        _containerName = configuration["AzureBlob:ContainerName"];
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        var blobClient = new BlobContainerClient(_connectionString, _containerName);
        await blobClient.CreateIfNotExistsAsync();

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var blob = blobClient.GetBlobClient(uniqueFileName);

        using (var stream = file.OpenReadStream())
        {
            await blob.UploadAsync(stream, overwrite: true);
        }

        return blob.Uri.ToString(); // You can store this in your DB
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
        {
            //throw new ArgumentException("File URL is invalid");
            return;
        }

        var blobClient = new BlobContainerClient(_connectionString, _containerName);
        await blobClient.CreateIfNotExistsAsync();

        // Extract blob name from full URL
        var uri = new Uri(fileUrl);
        var blobName = Path.GetFileName(uri.LocalPath); // safer than manually splitting by '/'

        var blob = blobClient.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync();
    }
}
