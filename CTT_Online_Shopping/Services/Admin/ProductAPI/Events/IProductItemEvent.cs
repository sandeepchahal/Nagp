using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public interface IProductItemEvent
{
    Task RaiseAddAsync(ProductItemDb productItemDb);
    Task RaiseUpdateAsync(ProductItemDb productItemDb);
}