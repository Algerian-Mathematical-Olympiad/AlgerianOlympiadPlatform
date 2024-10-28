namespace OlympTrain;

public class Problem(string id, ProblemSource source, DescriptionCollection descriptions, Difficulty difficulty)
{
    public ProblemSource Source { get; } = source;
    
    public DescriptionCollection Descriptions { get; } = descriptions;
    
    public Difficulty Difficulty { get; } = difficulty;

    public string GetId()
    {
        return id;
    }
    public new string ToString() => this.GetId();
}

public readonly struct ProblemSource(string name, string url)
{
    public string Name { get; } = name;
    public string Url { get; } = url;
}

