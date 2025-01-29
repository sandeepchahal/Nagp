using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Models.Abstract;

public abstract class ProductBase
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Category { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
}