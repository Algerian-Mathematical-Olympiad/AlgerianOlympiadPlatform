namespace DatabaseConnector.Models;

public class Problem
{
    public Problem(){}

    public Problem(string id, ProblemSource source, DescriptionCollection descriptions, Difficulty difficulty,
        int points = 30)
    {
        Id = id;
        Source = source;
        Descriptions = descriptions;
        Difficulty = difficulty;
        Points = points;
    }

    public string Id { get; set; } = "";

    public ProblemSource Source { get; set; } = new();

    public DescriptionCollection Descriptions { get; set; } = new();

    public Difficulty Difficulty { get; set; } = new();

    public int Points { get; set; } = 30;
}

public class ProblemSource
{
    public string Name { get; set; } = "";
    public string? Url { get; set; }
    
    public ProblemSource(){}

    public ProblemSource(string name, string url)
    {
        Name = name;
        Url = url;
    }
}