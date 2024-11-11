namespace DatabaseConnector.Models;

public struct Difficulty(int category, int hardness)
{
    private int Category { get; set; } = category;
    private int Hardness { get; set; } = hardness;

    public new string ToString()
    {
        return $"{Category} - {Hardness}";
    }
}
