using DatabaseConnector.Models;
using MongoDB.Driver;

namespace DatabaseConnector;

public class UnitManager(IMongoDatabase database) : DatabaseManager(database)
{
    public List<Unit> GetUnits()
    {
        var collection = Database.GetCollection<Unit>("units");
        return collection.Find(Builders<Unit>.Filter.Empty).ToList();
    }
    
    public void DeleteUnit(string id)
    {
        Database.GetCollection<Unit>("units").DeleteOne(pb => pb.Id == id);
    }
    
    public void CreateUnit(Unit unit)
    {
        Database.GetCollection<Unit>("units").InsertOne(unit);
    }
    
    public Unit GetUnitById(string id)
    {
        var problems = Database.GetCollection<Unit>("units");
        var problem = problems.Find(new ExpressionFilterDefinition<Unit>(pb => pb.Id == id)).ToList();
        if(problem == null || problem.Count == 0) throw new InvalidOperationException($"Unit does not exist");
        return problem[0];
    }
    
    public bool UnitExists(string id)
    {
        var problems = Database.GetCollection<Unit>("units");
        var problem = problems.Find(new ExpressionFilterDefinition<Unit>(pb => pb.Id == id)).ToList();
        if (problem == null || problem.Count == 0) return false;
        return true;
    }
    
    public void UpdateUnit(Unit unit, string id)
    {
        var filter = Builders<Unit>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.MakeUpdateDefinition(unit);
        
        Database.GetCollection<Unit>("units").UpdateOne(filter, update);
    }
    
    public List<IdOnly> GetUnitsIds()
    {
        var problems = Database.GetCollection<IdOnly>("units");
        return problems.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
    }
    
}