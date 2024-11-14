using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseConnector.Models;

[BsonIgnoreExtraElements]
public class IdOnly
{
    public string Id { get; set; } = "";
}