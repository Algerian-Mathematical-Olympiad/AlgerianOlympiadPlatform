namespace OlympTrain;

public class StudentSubmission(string id, Problem problem, Description description, List<IAttachment> attachments)
{
    public string Id { get; } = id;

    public Problem Problem { get; } = problem;

    public Description Description { get; } = description;

    public List<IAttachment> Attachments { get; } = attachments;
}

