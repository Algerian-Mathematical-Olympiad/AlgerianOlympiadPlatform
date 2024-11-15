using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class GroupManager(IMongoDatabase database) : DatabaseManager(database)
{

    public Group GetGroupById(string id)
    {
        var groups = Database.GetCollection<Group>("groups");
        var group = groups.Find(new ExpressionFilterDefinition<Group>(pb => pb.Id == id)).ToList();
        if(group == null || group.Count == 0) throw new InvalidOperationException($"Group does not exist");
        return group[0];
    }
    
    public bool GroupExists(string id)
    {
        var groups = Database.GetCollection<Group>("groups");
        var group = groups.Find(new ExpressionFilterDefinition<Group>(pb => pb.Id == id)).ToList();
        if (group == null || group.Count == 0) return false;
        return true;
    }

    public void CreateGroup(Group group)
    {
        if(GroupExists(group.Id)) throw new InvalidOperationException($"Group with same ID already exists");
        Database.GetCollection<Group>("groups").InsertOne(group);
    }

    public void UpdateGroup(Group group, string id)
    {
        var filter = Builders<Group>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.MakeUpdateDefinition(group);
        
        Database.GetCollection<Group>("groups").UpdateOne(filter, update);
    }

    public void DeleteGroup(string id)
    {
        Database.GetCollection<Group>("groups").DeleteOne(pb => pb.Id == id);
    }
    

    public List<Group> GetAllGroups()
    {
        var groups = Database.GetCollection<Group>("groups");
        return groups.Find(new ExpressionFilterDefinition<Group>(pb => true)).ToList();
    }
    
    public List<IdOnly> GetGroupsIds()
    {
        var groups = Database.GetCollection<IdOnly>("groups");
        return groups.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
    }
}

