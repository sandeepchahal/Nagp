using System.ComponentModel.DataAnnotations;
using ProductAPI.Models.Commands;
using ProductAPI.Models.DbModels;

namespace ProductAPI.Models.Abstract;

public abstract class ProductBase
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string BrandId { get; set; } = null!;  // Reference to Main Category

    [Required]
    public string CategoryId { get; set; } = null!;  // Reference to Main Category
    
    [Required]
    public string SubCategoryId { get; set; }= null!;  // Reference to Sub Category
}