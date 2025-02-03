using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.Commands;

public class CategoryCommand:CategoryBase
{ 
    public List<SubCategoryCommand> SubCategories { get; set; } = new();
}

public class SubCategoryCommand : SubCategoryBase
{
    public List<FilterAttribute> FilterAttributes { get; set; } = new();
}