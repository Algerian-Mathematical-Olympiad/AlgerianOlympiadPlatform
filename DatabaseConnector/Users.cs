using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class UserManager(IMongoDatabase database)
{
    private IMongoDatabase _database = database;

    public void CreateUser(DetailedUser user)
    {
        var users = _database.GetCollection<DetailedUser>("users");
        users.InsertOne(user);
    }

    public DetailedUser GetUserDetails(string emailOrUsername)
    {
        var users = _database.GetCollection<DetailedUser>("users");
        var user = users.Find(new ExpressionFilterDefinition<DetailedUser>(details => details.Email == emailOrUsername || details.Id == emailOrUsername)).ToList();
        if(user == null || user.Count == 0) throw new InvalidOperationException($"User {emailOrUsername} does not exist");
        return user[0];
    }
    
    public User GetUser(string emailOrUsername)
    {
        var users = _database.GetCollection<User>("users");
        var user = users.Find(new ExpressionFilterDefinition<User>(details => details.Email == emailOrUsername || details.Id == emailOrUsername)).ToList();
        if(user == null || user.Count == 0) throw new InvalidOperationException($"User {emailOrUsername} does not exist");
        return user[0];
    }

    public List<User> GetUsers()
    {
        var users = _database.GetCollection<User>("users");
        var userList = users.Find(new ExpressionFilterDefinition<User>(details => true)).ToList();
        
        return userList;
    }
    
    public List<DetailedUser> GetUsersWithDetails()
    {
        var users = _database.GetCollection<DetailedUser>("users");
        var userList = users.Find(new ExpressionFilterDefinition<DetailedUser>(details => true)).ToList();
        
        return userList;
    }

    public void DeleteUser(string emailOrUsername)
    {
        var users = _database.GetCollection<User>("users");
        users.DeleteOne(new ExpressionFilterDefinition<User>(details => details.Email == emailOrUsername || details.Id == emailOrUsername));
    }

    public bool DoesUserExist(string id)
    {
        var users = _database.GetCollection<User>("users");
        var user = users.Find(new ExpressionFilterDefinition<User>(u => u.Id == id)).ToList();
        if (user == null || user.Count == 0) return false;
        return true;

    }

    public void UpdateUser(DetailedUser user, string id)
    {
        _database.GetCollection<DetailedUser>("users").ReplaceOne(u => u.Id == id, user);

    }
}