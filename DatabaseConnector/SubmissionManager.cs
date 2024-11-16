using DatabaseConnector.Models;
using MongoDB.Driver;

namespace DatabaseConnector;

public class StudentSubmissionManager(IMongoDatabase database) : DatabaseManager(database)
{
    public List<StudentSubmission> GetStudentSubmissions()
    {
        var collection = Database.GetCollection<StudentSubmission>("StudentSubmissions");
        return collection.Find(Builders<StudentSubmission>.Filter.Empty).ToList();
    }
    
    public void DeleteStudentSubmission(string id)
    {
        Database.GetCollection<StudentSubmission>("StudentSubmissions").DeleteOne(pb => pb.Id == id);
    }
    
    public void CreateStudentSubmission(StudentSubmission studentSubmission)
    {
        Database.GetCollection<StudentSubmission>("StudentSubmissions").InsertOne(studentSubmission);
    }
    
    public StudentSubmission GetStudentSubmissionById(string id)
    {
        var problems = Database.GetCollection<StudentSubmission>("StudentSubmissions");
        var problem = problems.Find(new ExpressionFilterDefinition<StudentSubmission>(pb => pb.Id == id)).ToList();
        if(problem == null || problem.Count == 0) throw new InvalidOperationException($"StudentSubmission does not exist");
        return problem[0];
    }
    
    public bool StudentSubmissionExists(string id)
    {
        var problems = Database.GetCollection<StudentSubmission>("StudentSubmissions");
        var problem = problems.Find(new ExpressionFilterDefinition<StudentSubmission>(pb => pb.Id == id)).ToList();
        if (problem == null || problem.Count == 0) return false;
        return true;
    }
    
    public void UpdateStudentSubmission(StudentSubmission studentSubmission, string id)
    {
        var filter = Builders<StudentSubmission>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.MakeUpdateDefinition(studentSubmission);
        
        Database.GetCollection<StudentSubmission>("StudentSubmissions").UpdateOne(filter, update);
    }
    
    public List<IdOnly> GetStudentSubmissionsIds()
    {
        var problems = Database.GetCollection<IdOnly>("StudentSubmissions");
        return problems.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
    }
    
}