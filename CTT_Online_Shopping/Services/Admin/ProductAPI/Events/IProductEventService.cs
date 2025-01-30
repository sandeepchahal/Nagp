using ProductAPI.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public interface IProductEventService
{
    Task RaiseAddAsync(ProductDb productDb);
    Task RaiseUpdateAsync(ProductDb productDb);

}