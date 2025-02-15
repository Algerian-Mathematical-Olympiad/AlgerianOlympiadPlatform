using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class ProblemManager(IMongoDatabase database) : DatabaseManager(database)
{

    public Problem GetProblemById(string id)
    {
        var problems = Database.GetCollection<Problem>("problems");
        var problem = problems.Find(new ExpressionFilterDefinition<Problem>(pb => pb.Id == id)).ToList();
        if(problem == null || problem.Count == 0) throw new InvalidOperationException($"Problem does not exist");
        return problem[0];
    }
    
    public bool ProblemExists(string id)
    {
        var problems = Database.GetCollection<Problem>("problems");
        var problem = problems.Find(new ExpressionFilterDefinition<Problem>(pb => pb.Id == id)).ToList();
        if (problem == null || problem.Count == 0) return false;
        return true;
    }

    public void CreateProblem(Problem problem)
    {
        if(ProblemExists(problem.Id)) throw new InvalidOperationException($"Problem with same ID already exists");
        Database.GetCollection<Problem>("problems").InsertOne(problem);
    }

    public void UpdateProblem(Problem problem, string id)
    {
        var filter = Builders<Problem>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.MakeUpdateDefinition(problem);
        
        Database.GetCollection<Problem>("problems").UpdateOne(filter, update);
    }

    public void DeleteProblem(string id)
    {
        Database.GetCollection<Problem>("problems").DeleteOne(pb => pb.Id == id);
    }
    
    public List<Problem> GetAllProblems()
    {
        var problems = Database.GetCollection<Problem>("problems");
        return problems.Find(new ExpressionFilterDefinition<Problem>(pb => true)).ToList();
    }
    
    public List<IdOnly> GetProblemsIds()
    {
        var problems = Database.GetCollection<IdOnly>("problems");
        return problems.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
    }
}

