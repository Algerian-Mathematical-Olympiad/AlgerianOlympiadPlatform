using DatabaseConnector.Models;
using MongoDB.Driver;

namespace DatabaseConnector;

public class ProblemsetManager(IMongoDatabase database) : DatabaseManager(database)
{
    public List<MathProblemset> GetMathProblemsets()
    {
        var collection = Database.GetCollection<MathProblemset>("MathProblemsets");
        return collection.Find(Builders<MathProblemset>.Filter.Empty).ToList();
    }
    
    public void DeleteProblemset(string id)
    {
        Database.GetCollection<MathProblemset>("MathProblemsets").DeleteOne(pb => pb.Id == id);
    }
    
    public void CreateProblemset(MathProblemset problemset)
    {
        Database.GetCollection<MathProblemset>("MathProblemsets").InsertOne(problemset);
    }
    
    public MathProblemset GetProblemsetById(string id)
    {
        var problems = Database.GetCollection<MathProblemset>("MathProblemsets");
        var problem = problems.Find(new ExpressionFilterDefinition<MathProblemset>(pb => pb.Id == id)).ToList();
        if(problem == null || problem.Count == 0) throw new InvalidOperationException($"Problem does not exist");
        return problem[0];
    }
    
    public bool ProblemsetExists(string id)
    {
        var problems = Database.GetCollection<MathProblemset>("MathProblemsets");
        var problem = problems.Find(new ExpressionFilterDefinition<MathProblemset>(pb => pb.Id == id)).ToList();
        if (problem == null || problem.Count == 0) return false;
        return true;
    }
    
    public void UpdateProblemset(MathProblemset problemset, string id)
    {
        var filter = Builders<MathProblemset>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.MakeUpdateDefinition(problemset);
        
        Database.GetCollection<MathProblemset>("MathProblemsets").UpdateOne(filter, update);
    }
    
    public List<IdOnly> GetProblemsIds()
    {
        var problems = Database.GetCollection<IdOnly>("MathProblemsets");
        return problems.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
    }
    
}