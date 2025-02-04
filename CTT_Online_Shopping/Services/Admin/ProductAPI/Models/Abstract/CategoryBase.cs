using System.Text.Json.Serialization;

namespace ProductAPI.Models.Abstract;

public abstract class CategoryBase
{
    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;   
    
    [JsonPropertyName("mainCategory")]
    public string MainCategory { get; set; } = null!;
}

public class SubCategoryBase
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

}

public class SubCategoryDb : SubCategoryBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [JsonPropertyName("filterAttributes")]
    public List<FilterAttributeDb> FilterAttributes { get; set; } = new();
}
public class FilterAttribute
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
    
    [JsonPropertyName("options")]
    public List<string> Options { get; set; } = new();               
}
public class FilterAttributeDb:FilterAttribute
{
    public string Id { get; set; } = Guid.NewGuid().ToString();            
}


