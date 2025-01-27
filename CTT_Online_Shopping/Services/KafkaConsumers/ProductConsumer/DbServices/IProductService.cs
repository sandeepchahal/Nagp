using ProductConsumer.Models;

namespace ProductConsumer.DbServices;

public interface IProductService
{
    Task Add(Product product);
    Task Update(Product product);

}