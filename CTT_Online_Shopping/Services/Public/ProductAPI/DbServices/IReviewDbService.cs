using ProductAPI.Models.Review;

namespace ProductAPI.DbServices;

public interface IReviewDbService
{
    Task<bool> AddReview(ReviewDb reviewDb);
    Task<List<ReviewQuery>> GetByProductId(string productItemId);
}