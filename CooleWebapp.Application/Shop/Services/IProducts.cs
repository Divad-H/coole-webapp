using CooleWebapp.Application.Products.Services;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Shop.Services
{
  public record GetShopProductsRequestModel
  {
    public required UInt32 PageIndex { get; init; }
    public required UInt32 PageSize { get; init; }
  }

  public record ProductAmount
  {
    [Required] public required Int64 ProductId { get; init; }
    [Required] public required UInt32 Amount { get; init; }
    [Required] public required Decimal ExpectedPrice { get; init; }
  }

  public record BuyProductsRequestModel
  {
    [Required] public required IEnumerable<ProductAmount> Products { get; init; }
  }

  public record BuyProductsAsFridgeRequestModel
  {
    [Required] public required IEnumerable<ProductAmount> Products { get; init; }
    [Required] public required Int64 CoolUserId { get; init; }
    public string? PinCode { get; init; }
  }

  public interface IProducts
  {
    /// <summary>
    /// Read all products that are currently listed in the shop.
    /// </summary>
    Task<GetProductsResponseModel> ReadProducts(
      GetShopProductsRequestModel getShopProductsRequestModel,
      CancellationToken ct);

    Task<IReadOnlyCollection<ShortProductResponseModel>> ReadShortProducts(
      CancellationToken ct);

    Task<byte[]> ReadProductImage(Int64 productId, CancellationToken ct);

    Task BuyProducts(BuyProductsDto buyProductsDto, CancellationToken ct);
    Task BuyProductsAsFridge(BuyProductsAsFridgeDto buyProductsAsFridgeDto, CancellationToken ct);
  }
}
