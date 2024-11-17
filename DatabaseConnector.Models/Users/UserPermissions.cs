using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseConnector.Models;

[BsonIgnoreExtraElements]
public class UserPermissions
{
    public string Id { get; set; } = "";
    public bool IsStaff { get; set; }
    public bool IsSuperUser { get; set; }
}