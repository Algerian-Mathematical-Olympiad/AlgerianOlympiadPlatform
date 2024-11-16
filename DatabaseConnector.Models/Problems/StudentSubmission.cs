using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseConnector.Models;

public class StudentSubmission{
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public DateTime Timestamp { get; set; }

    public string User { get; set; } = "";

    public string Group { get; set; } = "";

    public string Problem { get; set; } = "";
    
    public Description Description { get; set; } = new();

    public List<string> Attachments { get; set; } = new();

    public List<Message> Conversation { get; set; } = new();

}