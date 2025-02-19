using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductAPI.DbServices;
using ProductAPI.Models.Common;
using ProductAPI.Models.Review;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/product/review")]
public class ReviewController(IReviewDbService reviewDbService) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddReview([FromBody] ReviewCommand reviewCommand)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token is missing");

            // convert reviewCommand to  reviewDb
            var reviewDb = await ConvertToDomainModel(reviewCommand, token);
            await reviewDbService.AddReview(reviewDb);
            return Ok(new {message = "Review has been added"});
        }
        catch (Exception)
        {
            return BadRequest("An error has occurred");
        }
    }

    [HttpGet("get/{productId}")]
    public async Task<IActionResult> GetByProductId(string productId)
    {
        var result = await reviewDbService.GetByProductId(productId);
        var final = result.OrderByDescending(col => col.CreatedAt).ToList();
        
        return Ok(final.Count()>5? final.Take(5): final);
    }

    private async Task<ReviewDb> ConvertToDomainModel(ReviewCommand command, string token)
    {
        var imageUrls = new List<ImageBinaryData>();

        if (command.Images != null && command.Images.Any())
        {
            imageUrls.AddRange(from image in command.Images
                where !string.IsNullOrEmpty(image.Base64Data)
                select Convert.FromBase64String(image.Base64Data)
                into imageBytes
                select new ImageBinaryData()
                {
                    Url = null, // Clear Base64 if no longer needed
                    ImageBinary = new BsonBinaryData(imageBytes) // Store as binary data
                });
        }
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
        return new ReviewDb()
        {
            ProductId = command.ProductId,
            ProductItemId = command.ProductItemId,
            Images = imageUrls,
            Comment = command.Comment,
            Rating = command.Rating,
            User = new User
            {
                UserId = userId!,
                Email = email!,
                UserName = fullName!,
            }
        };
    }
}