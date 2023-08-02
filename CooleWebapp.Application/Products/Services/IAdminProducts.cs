using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Products.Services;
public record ProductResponseModel
{
  [Required] public required UInt64 Id { get; set; }
  [MaxLength(256), Required] public required string Name { get; set; }
  public string? Description { get; set; }
  [Required] public required decimal Price { get; set; }
  [Required] public required ProductState State { get; set; }
}

public record ShortProductResponseModel
{
  [Required] public required UInt64 Id { get; set; }
  [MaxLength(256), Required] public required string Name { get; set; }
}

public record AddProductRequestModel
{
  [MaxLength(256), Required] public required string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  [Required] public required decimal Price { get; set; }
  [Required] public required ProductState State { get; set; }
}

public record EditProductRequestModel : AddProductRequestModel
{
  [Required] public required UInt64 ProductId { get; set; }
  /// <summary>
  /// The product image should not be updated and is not included in the request
  /// </summary>
  public required bool ImageUnchanged { get; set; }
}

public record GetProductsResponseModel
{
  [Required] public required Pagination Pagination { get; init; }
  [Required] public required IEnumerable<ProductResponseModel> Products { get; init; }
}

public record GetProductsRequestModel
{
  public required UInt32 PageIndex { get; init; }
  public required UInt32 PageSize { get; init; }
  public string? SearchFilter { get; init; }
  public ProductState? ProductStateFilter { get; init; }
  public required SortDirection SortDirection { get; init; }
}

public interface IAdminProducts
{
  Task<UInt64> CreateProduct(AddProductRequestModel addProductRequestModel,
    byte[]? productImage,
    CancellationToken ct);

  Task<GetProductsResponseModel> ReadProducts(
    GetProductsRequestModel getProductsRequestModel, 
    CancellationToken ct);

  Task<byte[]> ReadProductImage(UInt64 productId, CancellationToken ct);

  Task UpdateProduct(EditProductRequestModel editProductRequestModel,
    byte[]? productImage,
    CancellationToken ct);

  Task DeleteProduct(UInt64 productId, CancellationToken ct);
}
