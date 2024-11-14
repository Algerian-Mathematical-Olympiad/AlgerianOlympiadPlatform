using MongoDB.Driver;

namespace DatabaseConnector;

public class CloudAssetManager(IMongoDatabase database) : DatabaseManager(database)
{
    public List<Asset> GetAllAssets()
    {
        return database.GetCollection<Asset>("assets").AsQueryable().ToList();
    }

    public void AddAsset(string assetName)
    {
        database.GetCollection<Asset>("assets").InsertOne(new(assetName));
    }

    public void RemoveAsset(string assetName)
    {
        database.GetCollection<Asset>("assets").DeleteOne(x => x.Id == assetName);
    }
}

public class Asset(string assetName)
{
    public string Id { get; set; } = assetName;
}