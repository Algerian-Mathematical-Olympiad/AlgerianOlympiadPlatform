namespace OlympTrain;

public class Unit(
    string id,
    DescriptionCollection title,
    DescriptionCollection description,
    List<Lesson> lessons,
    IProblemset validationProblems,
    IProblemset problemset,
    List<IAttachment> attachments)
{
    public string Id { get; } = id;
    public DescriptionCollection Title { get; } = title;
    public DescriptionCollection Description { get; } = description;
    public List<Lesson> Lessons { get; } = lessons;
    public IProblemset ValidationProblems { get; } = validationProblems;
    public IProblemset Problemset { get; } = problemset;
    public List<IAttachment> Attachments { get; } = attachments;
}