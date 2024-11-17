using MongoDB.Driver;

namespace DatabaseConnector;

public class CloudAssetManager(IMongoDatabase database) : DatabaseManager(database)
{
    public List<Asset> GetAllAssets()
    {
        return Database.GetCollection<Asset>("assets").AsQueryable().ToList();
    }

    public void AddAsset(string assetName)
    {
        Database.GetCollection<Asset>("assets").InsertOne(new(assetName));
    }

    public void RemoveAsset(string? assetName)
    {
        Database.GetCollection<Asset>("assets").DeleteOne(x => x.Id == assetName);
    }
}

public class Asset(string assetName)
{
    public string Id { get; set; } = assetName;
}