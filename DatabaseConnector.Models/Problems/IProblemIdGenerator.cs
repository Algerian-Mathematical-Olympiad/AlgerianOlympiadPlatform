namespace DatabaseConnector.Models;

public interface IProblemIdGenerator
{
    public string GenerateFromStatement(string source, string statement);
}

public class FromSourceIdGenerator : IProblemIdGenerator
{
    public string GenerateFromStatement(string source, string statement)
    {
        return source.Replace(' ','_').ToLower();
    }
}