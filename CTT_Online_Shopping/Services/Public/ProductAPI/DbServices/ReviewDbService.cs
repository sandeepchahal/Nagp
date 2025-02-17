using MongoDB.Driver;
using ProductAPI.Models.Common;
using ProductAPI.Models.Review;

namespace ProductAPI.DbServices;

public class ReviewDbService(IMongoCollection<ReviewDb> reviewCollection):IReviewDbService
{
    public async Task<bool> AddReview(ReviewDb reviewDb)
    {
        await reviewCollection.InsertOneAsync(reviewDb);
        return true;
    }

    public async Task<List<ReviewQuery>> GetByProductId(string productItemId)
    {
        var reviews = await reviewCollection.Find(col=>col.ProductItemId == productItemId).ToListAsync();

        return reviews.ConvertAll(r => new ReviewQuery
        {
            Id = r.Id,
            ProductId = r.ProductId,
            ProductItemId = r.ProductItemId,
            User = r.User,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            IsVerifiedPurchase = r.IsVerifiedPurchase,
            Images = r.Images?.Select(col=>new Image64Bit()
            {
                Base64Data = Convert.ToBase64String(col.ImageBinary.Bytes),
                Url = ""
            }).ToList(),
            Replies = r.Replies,
            Dislike = r.Dislike,
            Like = r.Like,
        });
        
    }
}