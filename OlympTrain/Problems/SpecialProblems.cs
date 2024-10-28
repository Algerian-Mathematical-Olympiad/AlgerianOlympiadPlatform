namespace OlympTrain;

public class ProblemWithPredefinedAnswer(string id, ProblemSource source, DescriptionCollection descriptions, Difficulty difficulty, List<DescriptionCollection> possibleAnswers) : Problem(id, source, descriptions, difficulty)
{
    public List<DescriptionCollection> GetAnswers()
    {
        return possibleAnswers;
    }

    public bool IsAnswerCorrect(string answer, Language language)
    {
        foreach (var description in possibleAnswers)
        {
            if (description.GetDescription(language).Content == answer) return true;
        }
        return false;
    }
}

public class ProblemWithProposedAnswersProblemWithPredefinedAnswer(string id, ProblemSource source, DescriptionCollection descriptions, Difficulty difficulty, List<DescriptionCollection> possibleAnswers, List<DescriptionCollection> propositions) : ProblemWithPredefinedAnswer(id, source, descriptions, difficulty, possibleAnswers)
{
    public List<DescriptionCollection> GetPropositions()
    {
        return propositions;
    }
    
    public bool IsAnswerGroupCorrect(List<DescriptionCollection> answers, Language language)
    {
        foreach (var a in answers)
        {
            if(!possibleAnswers.Contains(a)) return false;
        }
        
        foreach (var a in possibleAnswers)
        {
            if(!answers.Contains(a)) return false;
        }

        return true;
    }
    
}