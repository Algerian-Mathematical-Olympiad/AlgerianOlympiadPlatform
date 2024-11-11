using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseConnector.Models;

[BsonIgnoreExtraElements]
public class User
{
    public string Id { get; set; }
    public UserName EnglishName { get; set; }
    public UserName ArabicName { get; set; }
    public SchoolYear SchoolYear { get; set; }
    public string Email { get; set; }

    public User(
        string id,
        UserName englishName,
        UserName arabicName,
        SchoolYear schoolYear,
        string email)
    {
        EnglishName = englishName;
        ArabicName = arabicName;
        SchoolYear = schoolYear;
        Email = email;
        Id = id;
    }
    
}

[BsonIgnoreExtraElements]
public class DetailedUser(
    string id,
    UserName englishName,
    UserName arabicName,
    DateOnly birthday,
    string email,
    School school,
    SchoolYear schoolYear,
    TshirtSize tshirtSize,
    bool hasPassport) : User(id, englishName, arabicName, schoolYear, email)
{
    public DateOnly Birthday { get; set; } = birthday;
    public School School { get; set; } = school;
    public TshirtSize TshirtSize { get; set; } = tshirtSize;
    public bool HasPassport { get; set; } = hasPassport;

    public User ToUndetailedUser()
    {
        return new User(Id, EnglishName, ArabicName, SchoolYear, Email);
    }
}

[BsonIgnoreExtraElements]
public struct UserName(string firstName, string lastName)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}