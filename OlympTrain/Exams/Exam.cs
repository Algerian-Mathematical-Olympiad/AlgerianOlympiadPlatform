namespace OlympTrain;

public class Exam
{
    public string Id { get; }
    public DescriptionCollection Name { get; }
    public DescriptionCollection Description { get; }
    public DateTime StartTime { get; }
    public int DurationInMinutes { get; }
    public List<User> Coaches { get; }
    public List<User> Correctors { get; }
    public List<Group> GroupsConcerned { get; }
    public IProblemset Problemset { get; }
    public Difficulty Difficulty { get; }
}