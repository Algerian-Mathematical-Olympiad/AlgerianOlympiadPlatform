namespace OlympTrain;

public class BlogArticle(string id, string content, List<IAttachment> attachments, IAttachment coverImage)
{
    public string Id { get; } = id;
    public string Content { get; } = content;
    public List<IAttachment> Attachments { get; } = attachments;
    public IAttachment CoverImage { get; } = coverImage;
}