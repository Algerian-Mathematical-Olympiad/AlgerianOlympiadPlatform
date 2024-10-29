using DatabaseConnector;
using OlympTrain;
using RaoufsConsole;
/*
var searcher = new AopsUtilities.ProblemSearcher();
Console.Write("Enter the name of the AoPS Problem please : ");
Problem p2 = searcher.GetFromAops(Console.ReadLine(), new HashGenerator());
Console.Write("Found :\n");
new ProblemDisplayer(p2).Display();
*/

var u = new UserManager(new TestDataBaseProvider().GetDatabase());

u.CreateUser(new(
    new("Larouf", "Ould Ali"),
    new("رؤوف", "ولد علي"),
    new DateOnly(2006,10,2),
    "raouf.ouldali@ensia.edu.dz",
    new School(new("ENSIA"), Wilaya.Alger, SchoolType.University),
    SchoolYear.UniversityUndergraduate,
    TshirtSize.S,
    true
    ));
