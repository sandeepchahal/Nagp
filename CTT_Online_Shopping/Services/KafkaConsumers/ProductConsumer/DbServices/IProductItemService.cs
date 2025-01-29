using ProductConsumer.Models;

namespace ProductConsumer.DbServices;

public interface IProductItemService
{
    Task Add(ProductItem productItem);
    Task Update(string id,ProductItem productItem);
}