using System.Text.Json.Serialization;
using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.Commands;

public class CategoryCommand:CategoryBase
{ 
    [JsonPropertyName("subCategories")]
    public List<SubCategoryCommand> SubCategories { get; set; } = new();
}

public class SubCategoryCommand : SubCategoryBase
{
    [JsonPropertyName("filterAttributes")]
    public List<FilterAttribute> FilterAttributes { get; set; } = new();
}