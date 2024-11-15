namespace DatabaseConnector.Models;

public class StudentSubmission(string id, Problem problem, Description description, List<string> attachments)
{
    public string Id { get; set; } = id;

    public Problem Problem { get; set; } = problem;

    public Description Description { get; set; } = description;

    public List<string> Attachments { get; set; } = attachments;
}

