using ProductAPI.Enums;

namespace ProductAPI.Models.Abstract;

public abstract class CategoryBase
{
    public string Gender { get; set; } = string.Empty;   
    public string MainCategory { get; set; } = null!;
}

public class SubCategoryBase
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public SubCategoryBase(string name, string gender, string? slug="")
    {
        Name = name;
        Slug = slug??GenerateSlug(gender, name);
    }
    private string GenerateSlug(string gender, string name)
    {
        return $"{gender}-{name}"
            .ToLower()
            .Replace(" ", "-")
            .Replace("&", "and")
            .Replace("/", "-");
    }
}

public class SubCategoryDb : SubCategoryBase
{
    public SubCategoryDb(string name, string gender, List<FilterAttributeDb> filterAttributes):base(name,gender)
    {
        FilterAttributes = filterAttributes;
    }
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<FilterAttributeDb> FilterAttributes { get; set; }
}
public class FilterAttribute
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public List<string> Options { get; set; } = new();               
}
public class FilterAttributeDb:FilterAttribute
{
    public string Id { get; set; } = Guid.NewGuid().ToString();            
}


