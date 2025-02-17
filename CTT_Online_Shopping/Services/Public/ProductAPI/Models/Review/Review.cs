using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProductAPI.Models.Common;

namespace ProductAPI.Models.Review;

public class ReviewBase
{
    public string ProductId { get; set; } = null!;
    public string ProductItemId { get; set; } = null!;
    public int Rating { get; set; }  
    public string? Comment { get; set; }
   
}


public class ReviewDb : ReviewBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public User User { get; set; } = new User(); // get from token
    public int Like { get; set; } = 0;
    public int Dislike { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsVerifiedPurchase { get; set; } = false;
    public List<RepliedComment>? Replies { get; set; } = new();
    public List<ImageBinaryData>? Images { get; set; }
}

public class ReviewCommand : ReviewBase
{
    public List<Image64Bit>? Images { get; set; }
}
public class ReviewQuery : ReviewBase
{
    public string Id { get; set; }
    public User User { get; set; } = new User(); // get from token
    public int Like { get; set; } = 0;
    public int Dislike { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsVerifiedPurchase { get; set; } = false;
    public List<RepliedComment>? Replies { get; set; } = new();
    public List<Image64Bit>? Images { get; set; }
    
}

public class RepliedComment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public User User { get; set; } = new User();  // User who replied
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

