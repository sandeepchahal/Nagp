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
                    productView.Images = GetImages(productItem!);
                    productView.Price = GetPrice(productItem!);
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

    private static List<ImagesBase> GetImages(ProductItem productItem)
    {
        if (productItem?.Variants == null)
            return new List<ImagesBase>();

        var images = new List<ImagesBase>();

        switch (productItem.VariantType)
        {
            case "Size":
                if (productItem.Variants.SizeVariant != null)
                {
                    images.AddRange(productItem.Variants.Images);
                 
                }

                break;

            case "Color":
                if (productItem.Variants.ColorVariant != null)
                {
                    images.AddRange(productItem.Variants.ColorVariant.Select(colorVariant => colorVariant.Image));
                }

                break;

            case "ColorAndSize":
                if (productItem.Variants.SizeColorVariant != null)
                {
                    images.AddRange(from sizeColorVariant in productItem.Variants.SizeColorVariant
                        where sizeColorVariant.Image.Url.Length > 0
                        select sizeColorVariant.Image);
                }

                break;

            default:
                break;
        }

        return images;
    }


    private static PriceBase GetPrice(ProductItem productItem)
    {
        if (productItem?.Variants == null)
            return new PriceBase();

        switch (productItem.VariantType)
        {
            case "Size":
                if (productItem.Variants.SizeVariant != null)
                {
                    var sizeVariant = productItem.Variants.SizeVariant.FirstOrDefault();
                    if (sizeVariant != null)
                    {
                        return new PriceBase
                        {
                            OriginalPrice = sizeVariant.Price,
                            DiscountPrice = (int)sizeVariant.DiscountedPrice,
                            Discount = sizeVariant.Discount ?? new Discount()
                        };
                    }
                }

                break;

            case "Color":
                if (productItem.Variants.ColorVariant != null)
                {
                    var colorVariant = productItem.Variants.ColorVariant.FirstOrDefault();
                    if (colorVariant != null)
                    {
                        return new PriceBase
                        {
                            OriginalPrice = colorVariant.Price,
                            DiscountPrice = (int)colorVariant.DiscountedPrice,
                            Discount = colorVariant.Discount ?? new Discount()
                        };
                    }
                }

                break;

            case "ColorAndSize":
                if (productItem.Variants.SizeColorVariant != null)
                {
                    var sizeColorVariant = productItem.Variants.SizeColorVariant.FirstOrDefault();
                    if (sizeColorVariant != null && sizeColorVariant.Sizes.Any())
                    {
                        var sizeVariant = sizeColorVariant.Sizes.First();
                        return new PriceBase
                        {
                            OriginalPrice = sizeVariant.Price,
                            DiscountPrice = (int)sizeVariant.DiscountedPrice,
                            Discount = sizeVariant.Discount ?? new Discount()
                        };
                    }
                }

                break;

            default:
                break;
        }

        return new PriceBase();
    }
}