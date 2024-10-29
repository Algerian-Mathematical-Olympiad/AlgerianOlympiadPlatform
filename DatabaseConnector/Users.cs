using DatabaseConnector.Models;
using MongoDB.Driver;
using OlympTrain;

namespace DatabaseConnector;

public class UserManager(IMongoDatabase database)
{
    private IMongoDatabase _database = database;

    public void CreateUser(DetailedUser user)
    {
        var users = _database.GetCollection<DbUserDetails>("users");
        users.InsertOne(DbUserDetails.From(user));
    }

    public DetailedUser GetUserDetails(string email)
    {
        var users = _database.GetCollection<DbUserDetails>("users");
        var user = users.Find(new ExpressionFilterDefinition<DbUserDetails>(details => details.Email == email)).ToList();
        if(user == null || user.Count == 0) throw new Exception($"User with email {email} does not exist");
        return user[0].ToDetailedUser();
    }
    
    public User GetUser(string email)
    {
        var users = _database.GetCollection<User>("users");
        var user = users.Find(new ExpressionFilterDefinition<User>(details => details.Email == email)).ToList();
        if(user == null || user.Count == 0) throw new Exception($"User with email {email} does not exist");
        return user[0];
    }
}