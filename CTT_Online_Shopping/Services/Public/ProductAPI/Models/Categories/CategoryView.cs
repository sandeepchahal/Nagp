namespace ProductAPI.Models.Categories;

public class CategoryView
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public string Gender { get; set; } = string.Empty;
    public List<SubCategoryView> SubCategories { get; set; } 
}

public class SubCategoryView
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = string.Empty;
}