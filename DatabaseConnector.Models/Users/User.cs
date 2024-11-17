using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseConnector.Models;

[BsonIgnoreExtraElements]
public class User
{
    [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Only letters, numbers, and underscores are allowed.")]
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
    bool hasPassport, Gender gender) : User(id, englishName, arabicName, schoolYear, email)
{
    public DetailedUser() : this("",new(), new(), DateOnly.MinValue, "", new School(), SchoolYear.Other, TshirtSize.M, false, Gender.Male)
    {
        
    }
    
    public DateOnly Birthday { get; set; } = birthday;
    public School School { get; set; } = school;
    public TshirtSize TshirtSize { get; set; } = tshirtSize;
    public bool HasPassport { get; set; } = hasPassport;

    public Gender Gender { get; set; } = gender;

    public User ToUndetailedUser()
    {
        return new User(Id, EnglishName, ArabicName, SchoolYear, Email);
    }
}

[BsonIgnoreExtraElements]
public class UserName(string firstName, string lastName)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;

    public UserName() : this("", "")
    {
        
    }
}

public enum Gender{
    Male,
    Female
}