using ProductAPI.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public interface IProductEvent
{
    Task RaiseAddProductAsync(ProductDb productDb);
}