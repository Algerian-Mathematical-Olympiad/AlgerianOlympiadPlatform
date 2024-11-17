using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class ExamManager(IMongoDatabase database) : DatabaseManager(database)
{

    public Exam GetExamById(string id)
    {
        var exams = Database.GetCollection<Exam>("Exams") ?? throw new ArgumentNullException("id");
        var exam = exams.Find(new ExpressionFilterDefinition<Exam>(x => x.Id == id)).ToList();
        if(exam == null || exam.Count == 0) throw new InvalidOperationException($"Exam does not exist");
        return exam[0];
    }
    
    public bool ExamExists(string id)
    {
        var exams = Database.GetCollection<Exam>("Exams");
        var exam = exams.Find(new ExpressionFilterDefinition<Exam>(x => x.Id == id)).ToList();
        if (exam == null || exam.Count == 0) return false;
        return true;
    }

    public void CreateExam(Exam exam)
    {
        if(ExamExists(exam.Id)) throw new InvalidOperationException($"Exam with same ID already exists");
        Database.GetCollection<Exam>("Exams").InsertOne(exam);
    }

    public void UpdateExam(Exam exam, string id)
    {
        var filter = Builders<Exam>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.MakeUpdateDefinition(exam);
        
        Database.GetCollection<Exam>("Exams").UpdateOne(filter, update);
    }

    public void DeleteExam(string id)
    {
        Database.GetCollection<Exam>("Exams").DeleteOne(x => x.Id == id);
    }
    

    public List<Exam> GetAllExams()
    {
        var exams = Database.GetCollection<Exam>("Exams");
        return exams.Find(new ExpressionFilterDefinition<Exam>(x => true)).ToList();
    }
    
    public List<IdOnly> GetExamsIds()
    {
        var exams = Database.GetCollection<IdOnly>("Exams");
        return exams.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
    }
}

