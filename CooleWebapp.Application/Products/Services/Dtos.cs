using CooleWebapp.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Products.Services;

public record ProductImageDto 
{
  public byte[]? Data { get; set; }
}

public record ProductDataDto
{
  [MaxLength(256), Required] public required string Name { get; set; }
  public string? Description { get; set; }
  public decimal Price { get; set; }
  public ProductState State { get; set; }
}

public record CreateProductDto : ProductDataDto
{
  public byte[]? Image { get; set; }
}

public record UpdateProductDto : ProductDataDto
{
  public required UInt64 Id { get; set; }
  public ProductImageDto? Image { get; set; }
}

public enum SortDirection
{
  ByNameAscending,
  ByNameDescending,
}
