using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AopWebAdmin.CloudStorage
{
    public interface ICloudStorage
    {
        Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage);
        Task DeleteFileAsync(string fileNameForStorage);
    }
}

public static class CloudStorage
{
    public static string GetUrlPrefix()
    {
        return "https://storage.googleapis.com/algerian_olympiad/";
    }
}