namespace OlympTrain;

public readonly struct Difficulty(int category, int hardness)
{
    private int Category { get; } = category;
    private int Hardness { get; } = hardness;

    public new string ToString()
    {
        return $"{Category} - {Hardness}";
    }
}
