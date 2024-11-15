using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class LessonManager(IMongoDatabase database) : DatabaseManager(database)
{

    public Lesson GetLessonById(string id)
    {
        var lessons = Database.GetCollection<Lesson>("lessons");
        var lesson = lessons.Find(new ExpressionFilterDefinition<Lesson>(pb => pb.Id == id)).ToList();
        if(lesson == null || lesson.Count == 0) throw new InvalidOperationException($"Lesson does not exist");
        return lesson[0];
    }
    
    public bool LessonExists(string id)
    {
        var lessons = Database.GetCollection<Lesson>("lessons");
        var lesson = lessons.Find(new ExpressionFilterDefinition<Lesson>(pb => pb.Id == id)).ToList();
        if (lesson == null || lesson.Count == 0) return false;
        return true;
    }

    public void CreateLesson(Lesson lesson)
    {
        if(LessonExists(lesson.Id)) throw new InvalidOperationException($"Lesson with same ID already exists");
        Database.GetCollection<Lesson>("lessons").InsertOne(lesson);
    }

    public void UpdateLesson(Lesson lesson, string id)
    {
        var filter = Builders<Lesson>.Filter
            .Eq(p => p.Id, id);
        var update = UpdateMaker.MakeUpdateDefinition(lesson);
        
        Database.GetCollection<Lesson>("lessons").UpdateOne(filter, update);
    }

    public void DeleteLesson(string id)
    {
        Database.GetCollection<Lesson>("lessons").DeleteOne(pb => pb.Id == id);
    }
    

    public List<Lesson> GetAllLessons()
    {
        var lessons = Database.GetCollection<Lesson>("lessons");
        return lessons.Find(new ExpressionFilterDefinition<Lesson>(pb => true)).ToList();
    }
    
    public List<IdOnly> GetLessonsIds()
    {
        var lessons = Database.GetCollection<IdOnly>("lessons");
        return lessons.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
    }
}

