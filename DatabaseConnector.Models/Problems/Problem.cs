namespace DatabaseConnector.Models;

public class Problem(string id, ProblemSource source, DescriptionCollection descriptions, Difficulty difficulty)
{
    public ProblemSource Source { get; set; } = source;
    
    public DescriptionCollection Descriptions { get; set; } = descriptions;
    
    public Difficulty Difficulty { get; set; } = difficulty;

    public string GetId()
    {
        return id;
    }
    public new string ToString() => this.GetId();
}

public struct ProblemSource(string name, string url)
{
    public string Name { get; set; } = name;
    public string Url { get; set; } = url;
}

