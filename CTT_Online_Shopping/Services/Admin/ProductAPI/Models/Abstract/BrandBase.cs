using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductAPI.Models.Abstract;

public class BrandBase
{
    public string Name { get; set; } = string.Empty;
    
}