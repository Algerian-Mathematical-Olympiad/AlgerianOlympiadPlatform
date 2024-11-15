using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class UserManager(IMongoDatabase database) : DatabaseManager(database)
{
    public void CreateUser(DetailedUser user)
    {
        var users = Database.GetCollection<DetailedUser>("users");
        users.InsertOne(user);
    }

    public DetailedUser GetUserDetails(string emailOrUsername)
    {
        var users = Database.GetCollection<DetailedUser>("users");
        var user = users.Find(new ExpressionFilterDefinition<DetailedUser>(details => details.Email == emailOrUsername || details.Id == emailOrUsername)).ToList();
        if(user == null || user.Count == 0) throw new InvalidOperationException($"User {emailOrUsername} does not exist");
        return user[0];
    }
    
    public User GetUser(string emailOrUsername)
    {
        var users = Database.GetCollection<User>("users");
        var user = users.Find(new ExpressionFilterDefinition<User>(details => details.Email == emailOrUsername || details.Id == emailOrUsername)).ToList();
        if(user == null || user.Count == 0) throw new InvalidOperationException($"User {emailOrUsername} does not exist");
        return user[0];
    }
    
    public UserAuthInfo GetUserAuthInfo(string emailOrUsername)
    {
        var users = Database.GetCollection<UserAuthInfo>("users");
        var user = users.Find(new ExpressionFilterDefinition<UserAuthInfo>(details => details.Email == emailOrUsername || details.Id == emailOrUsername)).ToList();
        if(user == null || user.Count == 0) throw new InvalidOperationException($"User {emailOrUsername} does not exist");
        return user[0];
    }

    public void ChangePassword(UserAuthInfo authInfo)
    {
        var users = Database.GetCollection<UserAuthInfo>("users");
        var filter = Builders<UserAuthInfo>.Filter
            .Eq(p => p.Id, authInfo.Id);
        var update = Builders<UserAuthInfo>.Update.Set(x => x.PasswordHash, authInfo.PasswordHash);
        users.UpdateOne(filter, update);
    }

    public List<IdOnly> GetUsersIds()
    {
        var users = Database.GetCollection<IdOnly>("users");
        var userList = users.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
        
        return userList;
    }
    
    public List<User> GetUsers()
    {
        var users = Database.GetCollection<User>("users");
        var userList = users.Find(new ExpressionFilterDefinition<User>(details => true)).ToList();
        
        return userList;
    }
    
    public List<DetailedUser> GetUsersWithDetails()
    {
        var users = Database.GetCollection<DetailedUser>("users");
        var userList = users.Find(new ExpressionFilterDefinition<DetailedUser>(details => true)).ToList();
        
        return userList;
    }

    public void DeleteUser(string emailOrUsername)
    {
        var users = Database.GetCollection<User>("users");
        users.DeleteOne(new ExpressionFilterDefinition<User>(details => details.Email == emailOrUsername || details.Id == emailOrUsername));
    }

    public bool DoesUserExist(string id)
    {
        var users = Database.GetCollection<User>("users");
        var user = users.Find(new ExpressionFilterDefinition<User>(u => u.Id == id)).ToList();
        if (user == null || user.Count == 0) return false;
        return true;

    }

    public void UpdateUser(DetailedUser user, string id)
    {
        var filter = Builders<DetailedUser>.Filter
            .Eq(p => p.Id, id);
        
        var update = UpdateMaker.MakeUpdateDefinition(user);
        
        Database.GetCollection<DetailedUser>("users").UpdateOne(filter, update);

    }
    
    
}