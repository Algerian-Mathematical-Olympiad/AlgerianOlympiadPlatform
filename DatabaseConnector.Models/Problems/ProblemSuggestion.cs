namespace DatabaseConnector.Models;

public class ProblemSuggestion(string id, User user, Problem problem)
{
    public string Id { get; set; } = id;
    public User User { get; set; } = user;
    public Problem Problem { get; set; } = problem;
}