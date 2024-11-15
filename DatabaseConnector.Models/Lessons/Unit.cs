namespace DatabaseConnector.Models;

public class Unit(
    string id,
    DescriptionCollection name,
    DescriptionCollection description,
    List<string> lessons,
    List<string> validationQuizzes,
    string problemset)
{
    public string Id { get; set; } = id;
    public DescriptionCollection Name { get; set; } = name;
    public DescriptionCollection Description { get; set; } = description;
    public List<string> Lessons { get; set; } = lessons;
    public List<string> ValidationQuizzes { get; set; } = validationQuizzes;
    public string Problemset { get; set; } = problemset;

    public Unit() : this("", new(), new(), [], [], "")
    {
        
    }
}