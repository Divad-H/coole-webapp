using CooleWebapp.Application.Products.Services;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Shop.Services
{
  public record GetShopProductsRequestModel
  {
    public UInt32 PageIndex { get; init; }
    public UInt32 PageSize { get; init; }
  }

  public record ProductAmount
  {
    [Required] public UInt64 ProductId { get; init; }
    [Required] public UInt32 Amount { get; init; }
    [Required] public Decimal ExpectedPrice { get; init; }
  }

  public record BuyProductsRequestModel
  {
    [Required] public IEnumerable<ProductAmount> Products { get; init; } = Array.Empty<ProductAmount>();
  }

  public interface IProducts
  {
    /// <summary>
    /// Read all products that are currently listed in the shop.
    /// </summary>
    Task<GetProductsResponseModel> ReadProducts(
      GetShopProductsRequestModel getShopProductsRequestModel,
      CancellationToken ct);

    Task<byte[]> ReadProductImage(UInt64 productId, CancellationToken ct);

    Task BuyProducts(BuyProductsDto buyProductsDto, CancellationToken ct);
  }
}
