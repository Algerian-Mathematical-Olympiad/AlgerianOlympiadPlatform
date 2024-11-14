using MongoDB.Driver;

namespace DatabaseConnector;

public abstract class DatabaseManager
{
    protected readonly IMongoDatabase Database;

    public DatabaseManager(IMongoDatabase database)
    {
        Database = database;
    }
}