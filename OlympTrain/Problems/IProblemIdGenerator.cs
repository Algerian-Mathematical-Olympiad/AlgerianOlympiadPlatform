namespace OlympTrain;

public interface IProblemIdGenerator
{
    public string GenerateFromStatement(string source, string statement);
}