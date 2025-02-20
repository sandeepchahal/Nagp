using MongoDB.Driver;
using ProductAPI.Enums;
using ProductAPI.Events;
using ProductAPI.Events.Models;
using ProductAPI.Helpers;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.DbServices;

public class ProductItemDbService(
    IMongoCollection<ProductItemDb> productItemCollection,
    IMongoCollection<ProductDb> productCollection,
    IMongoCollection<CategoryDb> categoryCollection,
    IBrandDbService brandDbService,
    IProductDbService productDbService,
    ICategoryDbService categoryDbService,
    IProductItemEventService productItemEventService):IProductItemDbService
{
    public async Task<ProductItemView?> GetAsync(string productItemId)
    {
        try
        {
            var productItem = await productItemCollection.FindAsync(col => col.Id == productItemId);
            var result = await productItem.FirstOrDefaultAsync();
            if (result is null)
                return null;
            var mapper = ProductItemMapper.MapToProductItemViewModel(result);
            var product = await productDbService.GetAsync(mapper.ProductId);
            mapper.Product = product??new ProductView();
            return mapper;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<ProductItemDb> AddAsync(ProductItemCommand productItem)
    {
        if (string.IsNullOrEmpty(productItem.ProductId))
            throw new KeyNotFoundException("Product Id not found");
        
        var productItemDb = ProductItemMapper.MapToDomainModel(productItem);
        await productItemCollection.InsertOneAsync(productItemDb);

        Task.Run(async () =>
        {
            try
            {
                var productItemAddEventModel = await MapToProductItemEvent(productItemDb);
                await productItemEventService.RaiseAddAsync(productItemAddEventModel);
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging mechanism)
                Console.WriteLine($"Error raising event: {ex.Message}");
            }
        });

        return productItemDb;
    }

    public async Task<int> RunAsync()
    {
        try
        {
            var productItems = await productItemCollection.Find(_ => true).ToListAsync();
            foreach (var item in productItems)
            {
                var productItemAddEventModel = await MapToProductItemEvent(item);
                await productItemEventService.RaiseAddAsync(productItemAddEventModel);
            }

            return productItems.Count;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    private async Task<ProductItemEventModel> MapToProductItemEvent(ProductItemDb productItemDb)
    {
        
        var product = await productCollection.Find(col=>col.Id == productItemDb.ProductId).FirstAsync();
        var category = await categoryCollection.Find(col => col.Id == product.CategoryId).FirstAsync();
        var brand = await brandDbService.GetAsync(product!.BrandId);
        var subCategory = category!.SubCategories.Find(col => col.Id == product.SubCategoryId);
        
        var result = new ProductItemEventModel()
        {
            ProductId = productItemDb.ProductId,
            ProductItemId = productItemDb.Id,
            VariantType = productItemDb.VariantType,
            Brand = brand.Name,
            Name = product.Name,
            Gender = category.Gender,
            SubCategoryId = subCategory!.Id,
            SubCategoryName = subCategory.Name,
            SubCategorySlug = subCategory.Slug,
            ColorVariant = productItemDb.VariantType == nameof(VariantTypeEnum.Color)
                ? productItemDb.Variant.ColorVariant!.
                    Select(col=>
                        new ProductVariantColorEventModel(){Color = col.Color,ColorId = col.Id}).ToList():null,
            SizeVariant = productItemDb.VariantType == nameof(VariantTypeEnum.Size)?
                productItemDb.Variant.SizeVariant!.Select(col=>new ProductVariantSizeEventModel()
                    {Size = col.Size,SizeId = col.Id}).ToList():null,
            SizeColorVariant = productItemDb.VariantType == nameof(VariantTypeEnum.ColorAndSize)?
                productItemDb.Variant.SizeColorVariant!.Select(col=> new ProductVariantSizeAndColorEventModel()
                {
                    Color = new ProductVariantColorEventModel(){Color = col.Color, ColorId = col.Id},
                    SizeAndColorId = col.Id,
                    Sizes = col.Sizes.Select(s=>new ProductVariantSizeEventModel()
                    {
                        Size = s.Size,
                        SizeId = s.Size
                    }).ToList()
                }).ToList(): null,
        };
        result.MinPrice = GetMinPrice(productItemDb);
        result.MaxPrice = GetMaxPrice(productItemDb);
        return result;
    }

    private int GetMinPrice(ProductItemDb productItemDb)
    {
        Enum.TryParse(productItemDb.VariantType, out VariantTypeEnum variantType);
        return variantType switch
        {
            VariantTypeEnum.Color => (int)productItemDb.Variant.ColorVariant.Min(col => col.Price),
            VariantTypeEnum.Size => (int)productItemDb.Variant.SizeVariant.Min(col => col.Price),
            _ => (int)productItemDb.Variant.SizeColorVariant.Select(col => 
                col.Sizes.Min(s => s.Price)).Min()
        };
    }
    private int GetMaxPrice(ProductItemDb productItemDb)
    {
        Enum.TryParse(productItemDb.VariantType, out VariantTypeEnum variantType);
        return variantType switch
        {
            VariantTypeEnum.Color => (int)productItemDb.Variant.ColorVariant.Max(col => col.Price),
            VariantTypeEnum.Size => (int)productItemDb.Variant.SizeVariant.Max(col => col.Price),
            _ => (int)productItemDb.Variant.SizeColorVariant.Select(col 
                => col.Sizes.Max(s => s.Price)).Max()
        };
    }
    
}