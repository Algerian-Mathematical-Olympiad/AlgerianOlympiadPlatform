namespace DatabaseConnector.Models;

public class Lesson(string id, DescriptionCollection name, DescriptionCollection content, List<string> attachments)
{
    public string Id { get; set; } = id;
    public DescriptionCollection Name { get; set; } = name;
    public DescriptionCollection Content { get; set; } = content;
    public List<string> Attachments { get; set; } = attachments;


    public Lesson() : this("",new(), new(), [])
    {
        
    }
}