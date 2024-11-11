using System.Globalization;

namespace DatabaseConnector.Models;

public class Description(string content, bool isRtl = false, Language language = Language.English)
{
    public bool IsRtl { get; set; } = isRtl;
    public Language Language { get; set; } = language;
    public string Content { get; set; } = content;

    protected bool Equals(Description other)
    {
        return IsRtl == other.IsRtl && Language == other.Language && Content == other.Content;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Description)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IsRtl, (int)Language, Content);
    }

    
    public static bool operator ==(Description a, Description b)
    {
        return a.Content == b.Content;
    }
    
    public static bool operator !=(Description a, Description b)
    {
        return a.Content != b.Content;
    }
}

public class DescriptionCollection(List<Description> descriptions)
{

    public Description GetDescription(Language language)
    {
        foreach (var d in descriptions.Where(d => d.Language == language))
            return d;
        
        throw new CultureNotFoundException($"Language {language} not found.");
    }
    
    protected bool Equals(DescriptionCollection other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((DescriptionCollection)obj);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
    
    public static bool operator ==(DescriptionCollection a, DescriptionCollection b)
    {
        return a.GetDescription(Language.English) == b.GetDescription(Language.English);
    }
    
    public static bool operator !=(DescriptionCollection a, DescriptionCollection b)
    {
        return a.GetDescription(Language.English) != b.GetDescription(Language.English);
    }

}