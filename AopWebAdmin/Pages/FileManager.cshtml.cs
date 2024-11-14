using AopWebAdmin.CloudStorage;
using DatabaseConnector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace AopWebAdmin.Pages;

public class FileManagerModel : PageModel
{
    private readonly ICloudStorage _storageClient;
    private readonly IMongoDatabase _database;
    
    public FileManagerModel(ICloudStorage cloudStorage, IMongoDatabase database)
    {
        _storageClient = cloudStorage;
        _database = database;
    }

    [BindProperty]
    public IFormFile File { get; set; }
    
    [BindProperty]
    public string FileTitle { get; set; }

    public string UploadResult { get; set; }
    
    public List<Asset> Assets { get; set; }
    
    [BindProperty]
    public string AssetToAffect { get; set; }
    
    [BindProperty]
    public Actions Action { get; set; }

    public async Task<IActionResult?> OnPostAsync()
    {
        switch (Action)
        {
            case Actions.Create:
                string fileName = FormFileName(FileTitle, File.FileName);
                new CloudAssetManager(_database).AddAsset(fileName);
                await _storageClient.UploadFileAsync(File, fileName);
                break;
            case Actions.Delete:
                await _storageClient.DeleteFileAsync(AssetToAffect);
                new CloudAssetManager(_database).RemoveAsset(AssetToAffect);
                break;
        }
        
        OnGet();
        return null;
    }
    

    private static string FormFileName(string title, string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);
        var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
        return "admin_uploads/" + fileNameForStorage;
    }


    
    public void OnGet()
    {
        Assets = new CloudAssetManager(_database).GetAllAssets();
    }

    public enum Actions
    {
        Delete,
        Create
    }
}