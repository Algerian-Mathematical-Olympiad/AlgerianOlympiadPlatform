namespace DatabaseConnector.Models;

public class Exam
{
    public string Id { get; set; } = "";
    public DescriptionCollection Name { get; set; } = new();
    public DescriptionCollection Description { get; set; } = new();
    public DateTime StartTime { get; set; } = DateTime.Now;
    public int DurationInMinutes { get; set; } = 270;
    public List<string> Correctors { get; set; } = [];
    public List<string> GroupsConcerned { get; set; } = [];
    public string Problemset { get; set; } = "";
    public Difficulty Difficulty { get; set; } = new();
}