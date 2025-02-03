namespace ProductAPI.Models.Query;

public class CategoryView
{
    public string Gender { get; set; } = null!;       
    public string MainCategoryName { get; set; } = null!;
    public string MainCategoryId { get; set; } = null!;
    public string SubCategoryName { get; set; } = null!;
    public string SubCategoryId { get; set; } = null!;
}