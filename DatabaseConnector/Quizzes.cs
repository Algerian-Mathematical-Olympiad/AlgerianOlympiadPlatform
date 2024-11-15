using MongoDB.Driver;
using DatabaseConnector.Models;

namespace DatabaseConnector;

public class QuizManager(IMongoDatabase database) : DatabaseManager(database)
{
    public Quiz GetQuizById(string id)
    {
        var quizzes = Database.GetCollection<Quiz>("quizzes");
        var quiz = quizzes.Find(new ExpressionFilterDefinition<Quiz>(qz => qz.Id == id)).ToList();
        if (quiz == null || quiz.Count == 0) throw new InvalidOperationException($"Quiz with ID {id} does not exist");
        return quiz[0];
    }

    public bool QuizExists(string id)
    {
        var quizzes = Database.GetCollection<Quiz>("quizzes");
        var quiz = quizzes.Find(new ExpressionFilterDefinition<Quiz>(qz => qz.Id == id)).ToList();
        return quiz != null && quiz.Count > 0;
    }

    public void CreateQuiz(Quiz quiz)
    {
        if (QuizExists(quiz.Id)) throw new InvalidOperationException($"Quiz with ID {quiz.Id} already exists");
        Database.GetCollection<Quiz>("quizzes").InsertOne(quiz);
    }

    public void UpdateQuiz(Quiz quiz, string id)
    {
        var filter = Builders<Quiz>.Filter.Eq(q => q.Id, id);
        var updateBuilder = new UpdateDefinitionBuilder<Quiz>();
        var update = updateBuilder.Set(x => x.Answers, quiz.Answers)
            .Set(x => x.Name, quiz.Name)
            .Set(x => x.Statement, quiz.Statement)
            .Set(x => x.Points, quiz.Points);

        Database.GetCollection<Quiz>("quizzes").UpdateOne(filter, update);
    }

    public void DeleteQuiz(string id)
    {
        Database.GetCollection<Quiz>("quizzes").DeleteOne(qz => qz.Id == id);
    }

    public List<Quiz> GetAllQuizzes()
    {
        var quizzes = Database.GetCollection<Quiz>("quizzes");
        return quizzes.Find(new ExpressionFilterDefinition<Quiz>(qz => true)).ToList();
    }

    public List<IdOnly> GetQuizzesIds()
    {
        var quizzes = Database.GetCollection<IdOnly>("quizzes");
        return quizzes.Find(new ExpressionFilterDefinition<IdOnly>(details => true)).ToList();
    }
}