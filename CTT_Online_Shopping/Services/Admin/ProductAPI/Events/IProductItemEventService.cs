using ProductAPI.Events.Models;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Events;

public interface IProductItemEventService
{
    Task RaiseAddAsync(ProductItemAddEventModel productItemAddEventModel);
    Task RaiseUpdateAsync(ProductItemAddEventModel productItemEventModel);
}