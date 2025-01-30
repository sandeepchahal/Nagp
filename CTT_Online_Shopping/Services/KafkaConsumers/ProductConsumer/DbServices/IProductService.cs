using ProductConsumer.Models;

namespace ProductConsumer.DbServices;

public interface IProductService
{
    Task Add(ProductEventModel productEventModel);
    Task Update(string id,ProductEventModel productEventModel);

}