namespace DatabaseConnector.Models;

public class Description
{
    public Description()
    {
        
    }
    public Description(string content, bool isRtl = false, Language language = Language.English)
    {
        IsRtl = isRtl;
        Language = language;
        Content = content;
    }

    public bool IsRtl { get; set; }
    public Language Language { get; set; }
    public string Content { get; set; } = "";

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
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return HashCode.Combine(IsRtl, (int)Language, Content);
        // ReSharper restore NonReadonlyMemberInGetHashCode
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

public class DescriptionCollection
{
    public Description EnglishDescription { get; set; } = new("");
    public Description ArabicDescription { get; set; } = new("");
    
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
        return a.EnglishDescription == b.EnglishDescription;
    }
    
    public static bool operator !=(DescriptionCollection a, DescriptionCollection b)
    {
        return a.EnglishDescription != b.EnglishDescription;
    }

}