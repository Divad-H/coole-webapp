using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Application.Products.Repository
{
  public interface IProductDataAccess
  {
    Task<UInt64> CreateProduct(Product product, CancellationToken ct);

    Task<Paginated<Product>> ReadProducts(
      Page page,
      string? searchFilter,
      ProductState? productStateFilter,
      SortDirection sortDirection,
      CancellationToken ct);

    Task<byte[]?> ReadProductImage(UInt64 productId, CancellationToken ct);

    Task UpdateProduct(Product product, CancellationToken ct);

    Task DeleteProduct(UInt64 productId, CancellationToken ct);
    Task DeleteProductImage(UInt64 productId, CancellationToken ct);
  }
}
