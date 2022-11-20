using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Products.Services;
public record ProductResponseModel
{
  [Required] public UInt64 Id { get; set; }
  [MaxLength(256), Required] public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  [Required] public decimal Price { get; set; }
  [Required] public ProductState State { get; set; }
}

public record AddProductRequestModel
{
  [MaxLength(256), Required] public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  [Required] public decimal Price { get; set; }
  [Required] public ProductState State { get; set; }
}

public record EditProductRequestModel : AddProductRequestModel
{
  [Required] public UInt64 ProductId { get; set; }
  /// <summary>
  /// The product image should not be updated and is not included in the request
  /// </summary>
  public bool ImageUnchanged { get; set; }
}

public record GetProductsResponseModel
{
  public GetProductsResponseModel(
    Pagination pagination,
    IEnumerable<ProductResponseModel> products)
  {
    Pagination = pagination;
    Products = products;
  }
  [Required] public Pagination Pagination { get; }
  [Required] public IEnumerable<ProductResponseModel> Products { get; }
}

public record GetProductsRequestModel
{
  public UInt32 PageIndex { get; init; }
  public UInt32 PageSize { get; init; }
  public string? SearchFilter { get; init; }
  public ProductState? ProductStateFilter { get; init; }
  public SortDirection SortDirection { get; init; }
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
