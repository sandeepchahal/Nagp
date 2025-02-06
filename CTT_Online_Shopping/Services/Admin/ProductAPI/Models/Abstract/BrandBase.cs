using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductAPI.Models.Abstract;

public class BrandBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
}