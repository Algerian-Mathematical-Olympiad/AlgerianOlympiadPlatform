namespace DatabaseConnector.Models;

public class Unit(
    string id,
    DescriptionCollection title,
    DescriptionCollection description,
    List<Lesson> lessons,
    IProblemset validationProblems,
    IProblemset problemset,
    List<IAttachment> attachments)
{
    public string Id { get; set; } = id;
    public DescriptionCollection Title { get; set; } = title;
    public DescriptionCollection Description { get; set; } = description;
    public List<Lesson> Lessons { get; set; } = lessons;
    public IProblemset ValidationProblems { get; set; } = validationProblems;
    public IProblemset Problemset { get; set; } = problemset;
    public List<IAttachment> Attachments { get; set; } = attachments;
}