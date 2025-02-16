using ProductAPI.Models.Categories;

namespace ProductAPI.Helper;

public static class CategoryHelper
{
    public static CategoryView MapToCategoryView(Category category)
    {
        return new CategoryView()
        {
            Id = category.Id,
            Name = category.MainCategory,
            Gender = category.Gender,
            SubCategories = category.SubCategories.Select(col=>new SubCategoryView()
            {
             Id   = col.Id,
             Name = col.Name,
             Slug = col.Slug
            }).ToList()   
        };
    }
    
    public static SubCategoryView MapToSubCategoryView(SubCategory subCategory)
    {
        return new SubCategoryView()
        {
            Id = subCategory.Id,
            Name = subCategory.Name,
            Slug = subCategory.Slug
        };
    }
}