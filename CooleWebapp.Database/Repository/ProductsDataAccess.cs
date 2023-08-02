using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using CooleWebapp.Core.Utilities;
using CooleWebapp.Database.Model;
using CooleWebapp.Database.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Database.Repository;

internal class ProductsDataAccess : IProductDataAccess
{
  private readonly WebappDbContext _dbContext;
  public ProductsDataAccess(WebappDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<UInt64> CreateProduct(Product product, CancellationToken ct)
  {
    var res = await _dbContext.Products.AddAsync(product, ct);
    return res.Entity.Id;
  }

  public async Task<byte[]?> ReadProductImage(ulong productId, CancellationToken ct)
  {
    return (await _dbContext.ProductImages
      .AsNoTracking()
      .SingleOrDefaultAsync(p => p.ProductId == productId))
      ?.Data;
  }

  public Task<Paginated<Product>> ReadProducts(
    Page page,
    string? searchFilter,
    ProductState? productStateFilter,
    SortDirection sortDirection,
    CancellationToken ct)
  {
    var query = _dbContext.Products
      .AsNoTracking();
    if (searchFilter is not null)
      query = query.Where(prod => prod.Name.ToLower()
        .Contains(searchFilter.ToLower()));
    if (productStateFilter is not null)
      query = query.Where(prod => prod.State == productStateFilter);
    if (sortDirection == SortDirection.ByNameAscending)
      query = query.OrderBy(prod => prod.Name);
    else
      query = query.OrderByDescending(prod => prod.Name);
    return query.Paginated(page, ct);
  }

  public async Task UpdateProduct(Product product, CancellationToken ct)
  {
    var dbProduct = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == product.Id, ct);
    if (dbProduct is null)
      throw new ClientError(ErrorType.NotFound, "Could not update product, because the product was not found.");
    dbProduct.Price = product.Price;
    dbProduct.State = product.State;
    dbProduct.Description = product.Description;
    dbProduct.Name = product.Name;
    if (product.ProductImage is not null)
      dbProduct.ProductImage = product.ProductImage;
  }

  public Task DeleteProduct(UInt64 productId, CancellationToken ct)
  {
    Product product = new() { Id = productId };
    _dbContext.Products.Attach(product);
    _dbContext.Products.Remove(product);
    return Task.CompletedTask;
  }

  public async Task DeleteProductImage(UInt64 productId, CancellationToken ct)
  {
    var productImage =
      await _dbContext.ProductImages.SingleOrDefaultAsync(pi => pi.ProductId == productId, ct);
    if (productImage is null)
      return;
    _dbContext.ProductImages.Attach(productImage);
    _dbContext.ProductImages.Remove(productImage);
  }

  public Task<Product?> GetProduct(ulong productId, CancellationToken ct)
  {
    return _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId, ct);
  }

  public Task<IQueryable<Product>> ReadAllProducts(CancellationToken ct)
  {
    return Task.FromResult<IQueryable<Product>>(_dbContext.Products);
  }
}
