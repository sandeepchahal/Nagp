using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductAPI.Models.Categories;

public class FilterAttribute
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public List<string> Options { get; set; } = new();               
}

public class SubCategory
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = string.Empty;
    public List<FilterAttribute> FilterAttributes { get; set; } = new();
}

public class Category
{ 
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string MainCategory { get; set; } = null!;
    public string Gender { get; set; } = string.Empty;  
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public List<SubCategory> SubCategories { get; set; } = new();
}