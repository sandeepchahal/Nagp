using ProductAPI.Events.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public interface IProductItemEventService
{
    Task RaiseAddAsync(ProductItemEventModel productItemEventModel);
    Task RaiseUpdateAsync(ProductItemEventModel productItemEventModel);
}