using ProductConsumer.Models;

namespace ProductConsumer.DbServices;

public interface IProductItemService
{
    Task Add(ProductItemEventModel productItemEventModel);
    Task Update(string id,ProductItemEventModel productItemEventModel);
}