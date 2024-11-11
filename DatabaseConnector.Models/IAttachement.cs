namespace DatabaseConnector.Models;

public interface IAttachment
{
    public Language Language { get; set; }
    public string GetUrl();
}