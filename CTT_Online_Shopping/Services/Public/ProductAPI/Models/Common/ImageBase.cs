using MongoDB.Bson;

namespace ProductAPI.Models.Common;

public class ImagesBase
{
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } = false;
    public int OrderNumber { get; set; }
}

public class Image64Bit
{
    public string Url { get; set; } = string.Empty;
    public string? Base64Data { get; set; } // Store as Base64 if needed
}

public class ImageBinaryData
{
    public string Url { get; set; } = string.Empty;
    public BsonBinaryData ImageBinary { get; set; }
}

public class Discount
{
    public string Type { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class User
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; }= string.Empty;
    public string Email { get; set; }= string.Empty;

}