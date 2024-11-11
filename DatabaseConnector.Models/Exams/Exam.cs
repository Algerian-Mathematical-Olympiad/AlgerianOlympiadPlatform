namespace DatabaseConnector.Models;

public class Exam
{
    public string Id { get; set; }
    public DescriptionCollection Name { get; set; }
    public DescriptionCollection Description { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationInMinutes { get; set; }
    public List<User> Coaches { get; set; }
    public List<User> Correctors { get; set; }
    public List<Group> GroupsConcerned { get; set; }
    public IProblemset Problemset { get; set; }
    public Difficulty Difficulty { get; set; }
}