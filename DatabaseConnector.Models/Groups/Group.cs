namespace DatabaseConnector.Models;

public class Group(
    string id,
    DescriptionCollection name,
    DescriptionCollection description,
    List<User> coaches,
    List<User> students,
    List<IProblemset> problemsets,
    Unit lessons)
{
    public string Id { get; set; } = id;
    public DescriptionCollection Name { get; set; } = name;
    public DescriptionCollection Description { get; set; } = description;
    public List<User> Coaches { get; set; } = coaches;
    public List<User> Students { get; set; } = students;
    public List<IProblemset> Problemsets { get; set; } = problemsets;
    public Unit Lessons { get; set; } = lessons;
}