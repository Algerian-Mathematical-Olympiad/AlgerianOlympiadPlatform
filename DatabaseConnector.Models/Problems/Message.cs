namespace DatabaseConnector.Models;

public class Message
{
    public string User { get; set; }
    public DateTime Timestamp { get; set; }
    public Description Description { get; set; }
}