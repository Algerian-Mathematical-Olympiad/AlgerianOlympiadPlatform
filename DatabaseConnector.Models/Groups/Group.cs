namespace DatabaseConnector.Models;

public class Group(
    string id,
    DescriptionCollection name,
    DescriptionCollection description,
    List<string> coaches,
    List<string> students,
    List<string> units)
{
    public string Id { get; set; } = id;
    public DescriptionCollection Name { get; set; } = name;
    public DescriptionCollection Description { get; set; } = description;
    public List<string> Coaches { get; set; } = coaches;
    public List<string> Students { get; set; } = students;
    public List<string> Units { get; set; } = units;

    public Group() : this("", new(), new(), [],[],[])
    {
        
    }
}