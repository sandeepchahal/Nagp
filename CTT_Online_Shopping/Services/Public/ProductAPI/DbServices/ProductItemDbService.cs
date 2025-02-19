using MongoDB.Driver;
using ProductAPI.Helper;
using ProductAPI.Models.Brands;
using ProductAPI.Models.ProductItems;
using ProductAPI.Models.Products;

namespace ProductAPI.DbServices;

public class ProductItemDbService(
    IMongoCollection<ProductItem> productItemCollection,
    IMongoCollection<Product> productCollection,
    IBrandDbService brandDbService,
    ICategoryDbService categoryDbService):IProductItemDbService
{
    public async Task<List<ProductItem>?> GetByProductIdAsync(string productId)
    {
        try
        {
            var result =
                await productItemCollection.Find(p => p.ProductId == productId).ToListAsync();
            return result;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ProductItemView?> GetAsync(string productItemId)
    {
        try
        {
            var productItem = await productItemCollection.FindAsync(col => col.Id == productItemId);
            var result = await productItem.FirstOrDefaultAsync();
            if (result is null)
                return null;
            var mapper = ProductItemHelper.MapToProductItemViewModel(result);
            return mapper;
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    public async Task<List<ProductItem>> GetAllAsync()
    {
        try
        {
            var result =
                await productItemCollection.Find(_ => true).ToListAsync();
            return result;

        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public async Task<ProductWithSimilarBrand> GetByBrand(string brandId)
    {
        try
        {
            var brand = await brandDbService.GetAsync(brandId);
            var productList = await productCollection.Find(col => col.BrandId == brandId).ToListAsync();
            var productViewList = new List<ProductView>();
            foreach (var product in productList)
            { 
                var productView = ProductHelper.MapToProductView(product);

                // Brand info
                productView.Brand = await brandDbService.GetAsync(product.BrandId) ?? new Brand();
                // product items
                var productItems = await GetByProductIdAsync(product.Id);
                foreach (var productItem in productItems)
                {
                    productView.Images = ProductHelper.GetImages(productItem!);
                    productView.Price = ProductHelper.GetPrice(productItem!);
                    productView.ProductItemId = productItem.Id;
                }
                productViewList.Add(productView);
            }
            
            return new ProductWithSimilarBrand(){Products = productViewList, Brand = brand};;
        }
        catch
        {
            throw;
        }
    }

    public async Task<ProductWithSimilarGenderView> GetByCategoryType(string categoryId)
    {
        try
        {
            var category = await categoryDbService.GetAsync(categoryId);
            var productList = await productCollection.Find(col => col.CategoryId == categoryId).ToListAsync();
            
            var productViewList = new List<ProductView>();
            foreach (var product in productList)
            { 
                var productView = ProductHelper.MapToProductView(product);

                // Brand info
                productView.Brand = await brandDbService.GetAsync(product.BrandId) ?? new Brand();
                // product items
                var productItems = await GetByProductIdAsync(product.Id);
                foreach (var productItem in productItems)
                {
                    productView.Images = ProductHelper.GetImages(productItem!);
                    productView.Price = ProductHelper.GetPrice(productItem!);
                    productView.ProductItemId = productItem.Id;
                }
                productViewList.Add(productView);
            }

            return new ProductWithSimilarGenderView() { Products = productViewList, Category = category };
        }
        catch
        {
            throw;
        }
    }

    public async Task<ProductWithSimilarChoiceView> GetBySubcategoryType(string subcategoryId)
    {
        try
        {
            var subCategory = await categoryDbService.GetSubCategoryIdAsync(subcategoryId);
            var productList = await productCollection.Find(col => col.SubCategoryId == subcategoryId).ToListAsync();
            var productViewList = new List<ProductView>();
            foreach (var product in productList)
            { 
                var productView = ProductHelper.MapToProductView(product);

                // Brand info
                productView.Brand = await brandDbService.GetAsync(product.BrandId) ?? new Brand();
                // Reviews Info
                // product items
                var productItems = await GetByProductIdAsync(product.Id);
                foreach (var productItem in productItems)
                {
                    productView.Images = ProductHelper.GetImages(productItem!);
                    productView.Price = ProductHelper.GetPrice(productItem!);
                    productView.ProductItemId = productItem.Id;
                }
                productViewList.Add(productView);
            }

            return new ProductWithSimilarChoiceView()
            {
                Products = productViewList,
                SubCategory = CategoryHelper.MapToSubCategoryView(subCategory)
            };
        }
        catch
        {
            throw;
        }
    }

}