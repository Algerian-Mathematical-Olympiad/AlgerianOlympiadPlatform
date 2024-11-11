namespace DatabaseConnector.Models;

public class BlogArticle(string id, string content, List<IAttachment> attachments, IAttachment coverImage)
{
    public string Id { get; set; } = id;
    public string Content { get; set; } = content;
    public List<IAttachment> Attachments { get; set; } = attachments;
    public IAttachment CoverImage { get; set; } = coverImage;
}