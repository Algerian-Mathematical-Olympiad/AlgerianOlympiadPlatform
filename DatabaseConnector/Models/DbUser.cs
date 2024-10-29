using MongoDB.Bson;
using OlympTrain;

namespace DatabaseConnector.Models;

public class DbUserDetails(UserName englishName, UserName arabicName, DateOnly birthday, string email, School school, SchoolYear schoolYear, TshirtSize tshirtSize, bool hasPassport) : DetailedUser(englishName, arabicName, birthday, email, school, schoolYear, tshirtSize, hasPassport)
{
    public ObjectId Id { get; set; }

    public static DbUserDetails From(DetailedUser detailedUser)
    {
        return new DbUserDetails(detailedUser.EnglishName, detailedUser.ArabicName,detailedUser.Birthday, detailedUser.Email, detailedUser.School, detailedUser.SchoolYear, detailedUser.TshirtSize, detailedUser.HasPassport);
    }

    public DetailedUser ToDetailedUser()
    {
        return new DetailedUser(englishName,arabicName,birthday,email,school,schoolYear,tshirtSize,hasPassport);
    }
}