using MongoDB.Driver;

namespace AopWebAdmin;

public class MongoDbProvider
{
    public IMongoDatabase GetDatabase(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        return database;
    }
    

}