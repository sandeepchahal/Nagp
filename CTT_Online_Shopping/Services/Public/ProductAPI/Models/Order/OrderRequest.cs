using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductAPI.Models.Order;


public class OrderDb: OrderRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Today;
}

public class OrderQuery
{
    public string Id { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string PaymentMode { get; set; } = null!;
    public decimal TotalCost { get; set; }
    public int ItemsCount { get; set; }
}

public class OrderDetailQuery: OrderQuery
{
    public List<CartItem> CartItems { get; set; } = new();
}

public class OrderRequest
{
    public List<CartItem> CartItems { get; set; } = new();
    public User User { get; set; }
    public string PaymentMode { get; set; }
    public decimal TotalCost { get; set; }
}


public class OrderConfirmed
{
    public string InvoiceId { get; set; }
    public string OrderStatus { get; set; }
}

public class CartItem
{
    public string ProductId { get; set; }
    public string ProductItemId { get; set; }
    public string VariantType { get; set; }
    public string SizeId { get; set; }
    public string SizeLabel { get; set; }
    public string ColorId { get; set; }
    public string ColorLabel { get; set; }
    public string ImgUrl { get; set; }
    public decimal DiscountedPrice { get; set; }
    public decimal Price { get; set; }
    public string Brand { get; set; }
    public string Name { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalPrice { get; set; }
    public int StockQuantity { get; set; }
}

public class User
{
    public PersonalInformation PersonalInformation { get; set; }
    public AddressDetail AddressDetail { get; set; }
}

public class PersonalInformation
{
    public string Name { get; set; }
    public string Email { get; set; }
    public long Phone { get; set; }
}

public class AddressDetail
{
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }
    public string Country { get; set; }
}

