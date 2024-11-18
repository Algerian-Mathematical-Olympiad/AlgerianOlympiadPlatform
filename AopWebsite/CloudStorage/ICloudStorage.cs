namespace AopWebsite.CloudStorage
{
    public interface ICloudStorage
    {
        Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage);
        Task DeleteFileAsync(string? fileNameForStorage);
    }
}

internal static class CloudStorage
{
    public static string GetUrlPrefix()
    {
        return "https://storage.googleapis.com/algerian_olympiad/";
    }
}