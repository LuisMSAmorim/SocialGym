using Microsoft.Azure;
using Azure.Storage.Blobs;

namespace SocialGym.CrossCutting;

public sealed class AzureStorage
{
    private BlobServiceClient BlobServiceClient { get; set; }
    private BlobContainerClient ContainerClient { get; set; }

    public AzureStorage()
    {
        this.Setup();
    }

    public async Task<string> Save(Stream buffer, string fileName)
    {
        BlobClient blobClient = ContainerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(buffer);

        return $"https://stinfnet001.blob.core.windows.net/images/{fileName}";
    }

    private void Setup()
    {
        string? connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

        BlobServiceClient = new BlobServiceClient(connectionString);

        string containerName = "images";

        ContainerClient = BlobServiceClient.GetBlobContainerClient(containerName);

        if(ContainerClient == null)
            ContainerClient = BlobServiceClient.CreateBlobContainer(containerName);
    }
}
