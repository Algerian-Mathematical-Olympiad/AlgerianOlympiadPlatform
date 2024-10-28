namespace OlympTrain;

public interface IAttachment
{
    public Language Language { get; }
    public string GetUrl();
}