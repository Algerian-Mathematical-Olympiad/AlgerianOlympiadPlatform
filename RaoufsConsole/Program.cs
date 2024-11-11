using DatabaseConnector;
using DatabaseConnector.Models;
using RaoufsConsole;

var searcher = new AopsUtilities.ProblemImporter();
Console.Write("Enter the name of the AoPS Problem please : ");
Problem p2 = searcher.ScrapProblemFromAops(Console.ReadLine(), new HashGenerator());
Console.Write("Found :\n");
new ProblemDisplayer(p2).Display();

/*
var u = new UserManager(new TestDataBaseProvider().GetDatabase());

var users = u.GetUsers();
foreach (var user in users)
{
    Console.WriteLine(user.Email);
}*/