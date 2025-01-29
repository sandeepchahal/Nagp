using ProductAPI.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public interface IProductEvent
{
    Task RaiseAddAsync(ProductDb productDb);
    Task RaiseUpdateAsync(ProductDb productDb);

}