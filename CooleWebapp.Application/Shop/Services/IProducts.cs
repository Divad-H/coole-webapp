using CooleWebapp.Application.Products.Services;

namespace CooleWebapp.Application.Shop.Services
{
  public record GetShopProductsRequestModel
  {
    public UInt32 PageIndex { get; init; }
    public UInt32 PageSize { get; init; }

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
  }
}
