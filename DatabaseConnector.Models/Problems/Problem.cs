using System.ComponentModel.DataAnnotations;

namespace DatabaseConnector.Models;

public class Problem
{
    public Problem() { }

    public Problem(string id, ProblemSource source, DescriptionCollection descriptions, Difficulty difficulty, int points = 30)
    {
        Id = id;
        Source = source;
        Descriptions = descriptions;
        Difficulty = difficulty;
        Points = points;
    }
    
    [Required]
    public string Id { get; set; }
    public ProblemSource Source { get; set; }
    
    public DescriptionCollection Descriptions { get; set; }
    
    public Difficulty Difficulty { get; set; }

    public int Points { get; set; } = 30;

}

public class ProblemSource(string name, string url)
{
    public string Name { get; set; } = name;
    public string Url { get; set; } = url;

    public ProblemSource() : this("", "")
    {
        
    }
    
}

