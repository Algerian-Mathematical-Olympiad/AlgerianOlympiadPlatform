using DatabaseConnector.Models;
using MongoDB.Driver;

namespace DatabaseConnector;

public static class UpdateMaker
{
    public static UpdateDefinition<T> MakeUpdateDefinition<T>(T element)
    {
        var updateDefinition = new List<UpdateDefinition<T>>();
        var updateBuilder = Builders<T>.Update;

        foreach (var property in typeof(T).GetProperties())
        {
            var propertyValue = property.GetValue(element);
            if (propertyValue != null) // Only add non-null properties to the update
            {
                var update = updateBuilder.Set(property.Name, propertyValue);
                updateDefinition.Add(update);
            }
        }

        return updateBuilder.Combine(updateDefinition);

    }
}