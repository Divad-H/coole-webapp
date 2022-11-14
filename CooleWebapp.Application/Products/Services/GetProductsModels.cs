using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Products.Services;

public record Product
{
  public UInt64 Id { get; set; }
  [MaxLength(256), Required] public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public decimal Price { get; set; }
  public byte[]? Image { get; set; }
  public ProductState State { get; set; }
}

public record GetProductsResponseModel
{
  public GetProductsResponseModel(
    Pagination pagination,
    IEnumerable<Product> products)
  {
    Pagination = pagination;
    Products = products;
  }
  [Required] public Pagination Pagination { get; }
  [Required] public IEnumerable<Product> Products { get; }
}

public enum SortDirection
{
  ByNameAscending,
  ByNameDescending,
  ByCreationTimeAscending,
  ByCreationTimeDescending,
}

public record GetProductsRequestModel
{
  public UInt32 PageIndex { get; init; }
  public UInt32 PageSize { get; init; }
  public string? SearchFilter { get; init; }
  public ProductState? ProductStateFilter { get; init; }
  public SortDirection SortDirection { get; init; }
}
