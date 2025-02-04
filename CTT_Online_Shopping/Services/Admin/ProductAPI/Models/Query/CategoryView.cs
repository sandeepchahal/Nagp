namespace ProductAPI.Models.Query;

public class CategoryViewBase
{
    public string Gender { get; set; } = null!;       
    public string Name { get; set; } = null!;
    public string Id { get; set; } = null!;
}
public class CategoryView:CategoryViewBase
{ 
    public List<SubCategoryView> SubCategories { get; set; } = new();
}