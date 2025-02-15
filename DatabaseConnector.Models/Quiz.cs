namespace DatabaseConnector.Models;

public class Quiz
{
    public string Id { get; set; } = "";
    public DescriptionCollection Name { get; set; } = new();
    public DescriptionCollection Statement { get; set; } = new();
    public Dictionary<string, QuizAnswer> Answers { get; set; } = new();
    public int Points { get; set; }
}

public class QuizAnswer
{
    public DescriptionCollection Descriptions { get; set; } = new();
    public bool IsCorrect { get; set; }
}