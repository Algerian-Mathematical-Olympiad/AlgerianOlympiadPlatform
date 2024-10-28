namespace OlympTrain;

public class Lesson(string id, DescriptionCollection name, DescriptionCollection content, List<IAttachment> attachments)
{
    public string Id { get; } = id;
    public DescriptionCollection Name { get; } = name;
    public DescriptionCollection Content { get; } = content;
    public List<IAttachment> Attachments { get; } = attachments;
}