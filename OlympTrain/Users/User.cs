namespace OlympTrain;

public class User
{
    public UserName EnglishName { get; }
    public UserName ArabicName { get; }
    public SchoolYear SchoolYear { get; }
    public string Email { get; set; }

    public User(
        UserName englishName,
        UserName arabicName,
        SchoolYear schoolYear,
        string email)
    {
        EnglishName = englishName;
        ArabicName = arabicName;
        SchoolYear = schoolYear;
        Email = email;
    }
    
}

public class DetailedUser(
    UserName englishName,
    UserName arabicName,
    DateOnly birthday,
    string email,
    School school,
    SchoolYear schoolYear,
    TshirtSize tshirtSize,
    bool hasPassport) : User(englishName, arabicName, schoolYear, email)
{
    public DateOnly Birthday { get; } = birthday;
    public School School { get; } = school;
    public TshirtSize TshirtSize { get; } = tshirtSize;
    public bool HasPassport { get; } = hasPassport;

    public User ToUndetailedUser()
    {
        return new User(EnglishName, ArabicName, SchoolYear, email);
    }
}

public struct UserName(string firstName, string lastName)
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}

