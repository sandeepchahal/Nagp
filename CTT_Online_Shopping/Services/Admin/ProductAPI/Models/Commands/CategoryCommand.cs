using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.Commands;

public class CategoryCommand:CategoryBase
{
    public List<SubCategoryCommand> SubCategories { get; set; } = new();

}

public class SubCategoryCommand : SubCategoryBase
{
    public SubCategoryCommand(string name, string gender, List<FilterAttribute> filterAttributes):base(name,gender)
    {
        FilterAttributes = filterAttributes;
    }
    public List<FilterAttribute> FilterAttributes { get; set; }
    public string? Gender { get; set; } = string.Empty;
}