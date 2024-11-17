using DatabaseConnector.Models;
using MongoDB.Driver;

namespace DatabaseConnector;

public class MathProblemset : IProblemset
{
    public string Id { get; set; } = "";
    public DescriptionCollection Name { get; set; } = new();
    public DescriptionCollection Description { get; set; } = new();
    public List<string> ProblemsIds { get; set; } = [];

    private IMongoDatabase? _database;

    public void SetDatabase(IMongoDatabase database)
    {
        _database = database;
    }
    
    public List<Problem> GetProblems()
    {
        if(_database == null)
            throw new NullReferenceException("Database is not set");
        var problems = _database.GetCollection<Problem>("problems");
        return problems.Find(Builders<Problem>.Filter.In(doc => doc.Id, ProblemsIds)).ToList();
    }
}