using MongoDB.Driver;
using ProductAPI.Helper;
using ProductAPI.Models.Order;

namespace ProductAPI.DbServices;

public class OrderDbService(IMongoCollection<OrderDb> orderCollection):IOrderDbService
{
    public async Task<OrderConfirmed> CreateOrderAsync(OrderRequest request)
    {
        try
        {
            var orderDb = OrderHelper.MapToDomainModel(request: request);
            
            await orderCollection.InsertOneAsync(orderDb);
            return new OrderConfirmed
            {
                InvoiceId = orderDb.Id,
                OrderStatus = "Completed"
            };
        }
        catch (Exception e)
        {
            return new OrderConfirmed() { OrderStatus = "Pending", InvoiceId = "" };
        }  
    }

    public async Task<List<OrderQuery>> GetOrdersAsync(string email)
    {
        try
        {
            var orders = await orderCollection
                .Find(o => o.User.PersonalInformation.Email == email)
                .SortByDescending(o => o.CreatedOn)
                .ToListAsync();

            return orders.Select(o => new OrderQuery
            {
                Id = o.Id,
                CreatedOn = o.CreatedOn,
                PaymentMode = o.PaymentMode,
                TotalCost = o.TotalCost,
                ItemsCount = o.CartItems.Count
            }).ToList();
        }
        catch (Exception e)
        {
            return [];
        }
    }

    public async Task<OrderDetailQuery> GetAsync(string id)
    {
        try
        {
            var order = await orderCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
            if (order == null) return new OrderDetailQuery();

            return new OrderDetailQuery
            {
                Id = order.Id,
                CreatedOn = order.CreatedOn,
                PaymentMode = order.PaymentMode,
                TotalCost = order.TotalCost,
                ItemsCount = order.CartItems.Count,
                CartItems = order.CartItems
            };
        }
        catch (Exception e)
        {
            return new OrderDetailQuery();
        }
    }
}