using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class ProblemManager(IMongoDatabase database)
{
    private IMongoDatabase _database = database;

    public Problem GetProblemById(string id)
    {
        var problems = _database.GetCollection<Problem>("problems");
        var problem = problems.Find(new ExpressionFilterDefinition<Problem>(pb => pb.Id == id)).ToList();
        if(problem == null || problem.Count == 0) throw new InvalidOperationException($"Problem does not exist");
        return problem[0];
    }
    
    public bool ProblemExists(string id)
    {
        var problems = _database.GetCollection<Problem>("problems");
        var problem = problems.Find(new ExpressionFilterDefinition<Problem>(pb => pb.Id == id)).ToList();
        if (problem == null || problem.Count == 0) return false;
        return true;
    }

    public void CreateProblem(Problem problem)
    {
        if(ProblemExists(problem.Id)) throw new InvalidOperationException($"Problem with same ID already exists");
        if(DoesProblemWithSourceLinkExist(problem.Source.Url)) throw new InvalidOperationException($"Problem with source link {problem.Source.Url} already exists");
        _database.GetCollection<Problem>("problems").InsertOne(problem);
    }

    public void UpdateProblem(Problem problem, string id)
    {
        var filter = Builders<Problem>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.Make(problem);
        
        _database.GetCollection<Problem>("problems").UpdateOne(filter, update);
    }

    public void DeleteProblem(string id)
    {
        _database.GetCollection<Problem>("problems").DeleteOne(pb => pb.Id == id);
    }

    private bool DoesProblemWithSourceLinkExist(string sourceLink)
    {
        var problems = _database.GetCollection<Problem>("problems");
        var problem = problems.Find(new ExpressionFilterDefinition<Problem>(pb => pb.Source.Url == sourceLink)).ToList();
        if (problem == null || problem.Count == 0) return false;
        return true;
    }

    public List<Problem> GetAllProblems()
    {
        var problems = _database.GetCollection<Problem>("problems");
        return problems.Find(new ExpressionFilterDefinition<Problem>(pb => true)).ToList();
    }
}