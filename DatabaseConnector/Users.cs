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

    public DetailedUser GetUserDetails(string email)
    {
        var users = _database.GetCollection<DetailedUser>("users");
        var user = users.Find(new ExpressionFilterDefinition<DetailedUser>(details => details.Email == email)).ToList();
        if(user == null || user.Count == 0) throw new Exception($"User with email {email} does not exist");
        return user[0];
    }
    
    public User GetUser(string email)
    {
        var users = _database.GetCollection<User>("users");
        var user = users.Find(new ExpressionFilterDefinition<User>(details => details.Email == email)).ToList();
        if(user == null || user.Count == 0) throw new Exception($"User with email {email} does not exist");
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

    public void ThrowUser(string email)
    {
        Console.WriteLine($"User with email {email} does not exist");
        var users = _database.GetCollection<User>("users");
        users.DeleteOne(new ExpressionFilterDefinition<User>(details => details.Email == email));
    }
}