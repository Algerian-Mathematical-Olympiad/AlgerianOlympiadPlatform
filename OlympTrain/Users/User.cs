namespace OlympTrain;

public class User(
    string id,
    UserName englishName,
    UserName arabicName,
    SchoolYear schoolYear)
{
    public string Id { get; } = id;
    public UserName EnglishName { get; } = englishName;
    public UserName ArabicName { get; } = arabicName;
    public SchoolYear SchoolYear { get; } = schoolYear;

}

public class DetailedUser(
    string id,
    UserName englishName,
    UserName arabicName,
    DateTime birthday,
    string email,
    School school,
    SchoolYear schoolYear,
    TshirtSize tshirtSize,
    bool hasPassport) : User(id, englishName, arabicName, schoolYear)
{
    public DateTime Birthday { get; } = birthday;
    public string Email { get; } = email;
    public School School { get; } = school;
    public TshirtSize TshirtSize { get; } = tshirtSize;
    public bool HasPassport { get; } = hasPassport;
}

public struct UserName(string firstName, string lastName)
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}

