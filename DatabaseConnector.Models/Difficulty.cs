namespace DatabaseConnector.Models;

public class Difficulty(int category, int hardness)
{
    public int Category { get; set; } = category;
    public int Hardness { get; set; } = hardness;

    public Difficulty() : this(1,1)
    {
        
    }

    public new string ToString()
    {
        return $"{Category} - {Hardness}";
    }
}
