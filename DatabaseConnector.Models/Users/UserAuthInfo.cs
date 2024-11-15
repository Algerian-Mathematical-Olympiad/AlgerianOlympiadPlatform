using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseConnector.Models;

[BsonIgnoreExtraElements]
public class UserAuthInfo
{
    public string Id { get; set; } = "";
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsSuperUser { get; set; } = false;

}