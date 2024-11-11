namespace DatabaseConnector.Models;

public class School
{
    public School(Description name, Wilaya wilaya, SchoolType type)
    {
        Name = name;
        Wilaya = wilaya;
        Type = type;
    }

    public Description Name { get; set; }
    public Wilaya Wilaya { get; set; }
    public SchoolType Type { get; set; }
}

public enum SchoolType
{
    PrimarySchool,
    MiddleSchool,
    HighSchool,
    University,
    Other
}

public enum SchoolYear
{
    Primary1,
    Primary2,
    Primary3,
    Primary4,
    Primary5,
    Middle1,
    Middle2,
    Middle3,
    Middle4,
    High1,
    High2,
    High3,
    UniversityUndergraduate,
    UniversityGraduate,
    Other
}