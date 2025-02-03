using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.DbModels;

public class CategoryDb:CategoryBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public List<SubCategoryDb> SubCategories { get; set; } = new();

}
