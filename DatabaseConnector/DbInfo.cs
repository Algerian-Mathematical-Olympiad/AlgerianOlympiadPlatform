using MongoDB.Bson;
using MongoDB.Driver;

namespace DatabaseConnector;

public class TestDataBaseProvider
{
    public IMongoDatabase GetDatabase()
    {
        var client = new MongoClient("mongodb://myUserAdmin:abc123@localhost:27017");
        var database = client.GetDatabase("AlgerianMO");
        return database;
    }
    

}