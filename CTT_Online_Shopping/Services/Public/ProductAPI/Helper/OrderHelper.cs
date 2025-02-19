using ProductAPI.Models.Order;

namespace ProductAPI.Helper;

public static class OrderHelper
{
    public static OrderDb MapToDomainModel(OrderRequest request)
    {
        return new OrderDb()
        {
            User = request.User,
            CartItems = request.CartItems,
            PaymentMode = request.PaymentMode,
            TotalCost = request.TotalCost
        };
    }
}