namespace DatabaseConnector.Models;

public class Lesson(string id, DescriptionCollection name, DescriptionCollection content, List<IAttachment> attachments)
{
    public string Id { get; set; } = id;
    public DescriptionCollection Name { get; set; } = name;
    public DescriptionCollection Content { get; set; } = content;
    public List<IAttachment> Attachments { get; set; } = attachments;
}