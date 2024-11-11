namespace DatabaseConnector.Models;

public interface IProblemset
{
    public string Id { get; set; }
    public DescriptionCollection Name { get; set; }
    public DescriptionCollection Description { get; set; }
    public List<Description> Descriptions { get; set; }
    public List<Problem> GetProblems();
}