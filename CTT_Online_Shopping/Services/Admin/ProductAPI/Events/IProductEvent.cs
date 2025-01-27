using ProductAPI.Models;

namespace ProductAPI.Events;

public interface IProductEvent
{
    Task RaiseAddProductAsync(Product product);
}