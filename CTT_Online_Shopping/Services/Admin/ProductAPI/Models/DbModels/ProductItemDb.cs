using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.DbModels;

public class ProductItemDb:ProductItemBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public bool IsDiscountApplied { get; set; } = false;
    public List<ProductVariant> Variants { get; set; } = new();

}
