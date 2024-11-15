using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class UserPermissionsManager(IMongoDatabase database) : DatabaseManager(database)
{

    public UserPermissions GetUserPermissionsById(string id)
    {
        var users = Database.GetCollection<UserPermissions>("users");
        var userPermissions = users.Find(new ExpressionFilterDefinition<UserPermissions>(pb => pb.Id == id)).ToList();
        if(userPermissions == null || userPermissions.Count == 0) throw new InvalidOperationException($"UserPermissions does not exist");
        return userPermissions[0];
    }
    
    public void UpdateUserPermissions(UserPermissions userPermissions, string id)
    {
        var filter = Builders<UserPermissions>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.MakeUpdateDefinition(userPermissions);
        
        Database.GetCollection<UserPermissions>("users").UpdateOne(filter, update);
    }

    public List<UserPermissions> GetStaff()
    {
        var users = Database.GetCollection<UserPermissions>("users");
        return users.Find(x => x.IsStaff).ToList();
    }
    
    public List<UserPermissions> GetStudents()
    {
        var users = Database.GetCollection<UserPermissions>("users");
        return users.Find(x => !x.IsStaff).ToList();
    }
}