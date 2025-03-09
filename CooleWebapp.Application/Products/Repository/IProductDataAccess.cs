using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Application.Products.Repository
{
  public interface IProductDataAccess
  {
    Task<Int64> CreateProduct(Product product, CancellationToken ct);

    Task<Paginated<Product>> ReadProducts(
      Page page,
      string? searchFilter,
      ProductState? productStateFilter,
      SortDirection sortDirection,
      CancellationToken ct);

    Task<IQueryable<Product>> ReadAllProducts(CancellationToken ct);

    Task<byte[]?> ReadProductImage(Int64 productId, CancellationToken ct);

    Task UpdateProduct(Product product, CancellationToken ct);

    Task DeleteProduct(Int64 productId, CancellationToken ct);
    Task DeleteProductImage(Int64 productId, CancellationToken ct);
    Task<Product?> GetProduct(Int64 productId, CancellationToken ct);
  }
}
