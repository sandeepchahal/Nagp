using ProductAPI.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public interface IProductEventService
{
    Task RaiseUpdateAsync(ProductDb productDb);

}