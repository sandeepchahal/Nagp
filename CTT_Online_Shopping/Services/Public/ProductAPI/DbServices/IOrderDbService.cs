using ProductAPI.Models.Order;

namespace ProductAPI.DbServices;

public interface IOrderDbService
{
    Task<OrderConfirmed> CreateOrderAsync(OrderRequest request);
    Task<List<OrderQuery>> GetOrdersAsync(string email);
    Task<OrderDetailQuery> GetAsync(string id);
}