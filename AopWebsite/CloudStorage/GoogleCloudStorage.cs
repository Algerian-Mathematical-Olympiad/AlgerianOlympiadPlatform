
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace AopWebsite.CloudStorage;

public class GoogleCloudStorage : ICloudStorage
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public GoogleCloudStorage(IConfiguration configuration)
    {
        var googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
        _storageClient = StorageClient.Create(googleCredential);
        _bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket")!;
    }

    public async Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage)
    {
        using var memoryStream = new MemoryStream();
        await imageFile.CopyToAsync(memoryStream);
        var dataObject = await _storageClient.UploadObjectAsync(_bucketName, fileNameForStorage, null, memoryStream);
        return dataObject.MediaLink;
    }

    public async Task DeleteFileAsync(string? fileNameForStorage)
    {
        await _storageClient.DeleteObjectAsync(_bucketName, fileNameForStorage);
    }
}