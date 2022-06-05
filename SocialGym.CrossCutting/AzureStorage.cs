using Microsoft.Azure;
using Azure.Storage.Blobs;

namespace SocialGym.CrossCutting;

public sealed class AzureStorage
{
    private BlobServiceClient blobServiceClient { get; set; }
    private BlobContainerClient containerClient { get; set; }

    public AzureStorage()
    {
        Setup();
    }

    public async Task<string> Save(Stream buffer, string fileName)
    {
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(buffer);

        return $"https://stinfnet001.blob.core.windows.net/images/{fileName}";
    }

    private void Setup()
    {
        string? connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

        blobServiceClient = new BlobServiceClient(connectionString);

        string containerName = "images";

        containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        if(containerClient == null)
            containerClient = blobServiceClient.CreateBlobContainer(containerName);
    }
}
