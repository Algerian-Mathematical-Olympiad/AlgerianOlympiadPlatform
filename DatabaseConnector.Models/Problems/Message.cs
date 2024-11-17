namespace DatabaseConnector.Models;

public class Message
{
    public required string User { get; set; }
    public DateTime Timestamp { get; set; }
    public required Description Description { get; set; }
}