using System.ComponentModel.DataAnnotations;
using ProductAPI.Models.Abstract;

namespace ProductAPI.Models.Commands;

public class ProductCommand : ProductBase
{
    [Required]
    public BrandCommand Brand { get; set; } = null!; 
}