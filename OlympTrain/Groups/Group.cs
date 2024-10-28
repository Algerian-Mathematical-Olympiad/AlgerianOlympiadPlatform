namespace OlympTrain;

public class Group(
    string id,
    DescriptionCollection name,
    DescriptionCollection description,
    List<User> coaches,
    List<User> students,
    List<IProblemset> problemsets,
    Unit lessons)
{
    public string Id { get; } = id;
    public DescriptionCollection Name { get; } = name;
    public DescriptionCollection Description { get; } = description;
    public List<User> Coaches { get; } = coaches;
    public List<User> Students { get; } = students;
    public List<IProblemset> Problemsets { get; } = problemsets;
    public Unit Lessons { get; } = lessons;
}