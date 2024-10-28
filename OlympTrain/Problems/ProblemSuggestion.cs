namespace OlympTrain;

public class ProblemSuggestion(string id, User user, Problem problem)
{
    public string Id { get; } = id;
    public User User { get; } = user;
    public Problem Problem { get; } = problem;
}