namespace DatabaseConnector.Models;

public class BlogArticle(string id, string content, List<string> attachments, string coverImage)
{
    public string Id { get; set; } = id;
    public string Content { get; set; } = content;
    public List<string> Attachments { get; set; } = attachments;
    public string CoverImage { get; set; } = coverImage;
}