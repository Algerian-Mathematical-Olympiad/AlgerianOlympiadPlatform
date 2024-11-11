namespace DatabaseConnector.Models;

public interface IProblemIdGenerator
{
    public string GenerateFromStatement(string source, string statement);
}