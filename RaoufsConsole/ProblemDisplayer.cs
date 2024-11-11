using DatabaseConnector.Models;
namespace RaoufsConsole;

public class ProblemDisplayer(Problem problem)
{
    public void Display()
    {
        Console.WriteLine(problem.Source.Name);
        Console.WriteLine("Taken from " + problem.Source.Url);
        Console.WriteLine("ID : " + problem.GetId());
        Console.WriteLine("Difficulty : " + problem.Difficulty.ToString());
        Console.WriteLine(problem.Descriptions.GetDescription(language: Language.English).Content);
    }
}