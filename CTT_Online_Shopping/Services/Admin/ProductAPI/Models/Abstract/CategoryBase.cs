namespace ProductAPI.Models.Abstract;

public abstract class CategoryBase
{
    public string Gender { get; set; } = string.Empty;   
    public string MainCategory { get; set; } = null!;
}

public class SubCategoryBase
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = string.Empty;

}

public class SubCategoryDb : SubCategoryBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<FilterAttributeDb> FilterAttributes { get; set; } = new();
}
public class FilterAttribute
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public List<string> Options { get; set; } = new();               
}
public class FilterAttributeDb:FilterAttribute
{
    public string Id { get; set; } = Guid.NewGuid().ToString();            
}


