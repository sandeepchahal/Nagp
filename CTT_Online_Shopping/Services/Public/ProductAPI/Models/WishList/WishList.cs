using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Models.Brands;

namespace ProductAPI.Models.WishList;

public class WishListBase
{
    public string ProductId { get; set; }
    public string ProductItemId { get; set; }

}

public class WishListDb : WishListBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Email { get; set; }
    
}

public class WishListQuery : WishListBase
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Brand Brand { get; set; }
    public string ImageUrl { get; set; }= null!;
    public int Price { get; set; }
}