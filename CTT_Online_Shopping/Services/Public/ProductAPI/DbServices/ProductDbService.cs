using MongoDB.Driver;
using ProductAPI.Helper;
using ProductAPI.Models.Brands;
using ProductAPI.Models.Common;
using ProductAPI.Models.ProductItems;
using ProductAPI.Models.Products;

namespace ProductAPI.DbServices;

public class ProductDbService(
    IMongoCollection<Product> productCollection,
    IProductItemDbService productItemDbService,
    IBrandDbService brandDbService,
    ICategoryDbService categoryDbService
) : IProductDbService
{
    public async Task<Product?> GetAsync(string id)
    {
        try
        {
            var result = await productCollection.FindAsync(p => p.Id == id);
            var product = await result.FirstOrDefaultAsync();
            if (product is null) return null;
            var productItems = await productItemDbService.GetByProductIdAsync(id);
            //product.Items = productItems;
            return product;
            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<Product>> GetAll()
    {
        try
        {
            var products = await productCollection.Find(_ => true).ToListAsync();
            return products;
        }
        catch (Exception e)
        {
            throw;
        }
        
    }

    public async Task<List<ProductView>> GetBySubCategorySlugAsync(string slug)
    {
        try
        {
            var subCategory = await categoryDbService.GetSubCategoryAsync(slug);
            if (subCategory == null)
                return new List<ProductView>();
            
            var productList = await productCollection.Find(col => col.SubCategoryId == subCategory.Id).ToListAsync();
            var result = new List<ProductView>();

            foreach (var product in productList)
            {
                var productView = ProductHelper.MapToProductView(product);

                // Brand info
                productView.Brand = await brandDbService.GetAsync(product.BrandId) ?? new Brand();
                // product items
                var productItems = await productItemDbService.GetByProductIdAsync(product.Id);
                foreach (var productItem in productItems)
                {
                    productView.Images = ProductHelper.GetImages(productItem!);
                    productView.Price = ProductHelper.GetPrice(productItem!);
                    productView.ProductItemId = productItem.Id;
                }

                result.Add(productView);
            }

            return result;
        }
        catch
        {
            throw;
        }
    }

   
}