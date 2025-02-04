using ProductAPI.Models.DbModels;
using ProductAPI.Models.Query;

namespace ProductAPI.Helpers;

public static class CategoryHelper
{
   public static CategoryView MapToCategoryView(CategoryDb categoryDb)
   {
      return new CategoryView()
      {
         Gender = categoryDb.Gender,
         Id = categoryDb.Id,
         Name = categoryDb.MainCategory,
         SubCategories = categoryDb.SubCategories.Select(col => new SubCategoryView()
            { Id = col.Id, Name = col.Name }
         ).ToList(),
      };
   }
}