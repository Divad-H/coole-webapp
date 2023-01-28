using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.ErrorHandling;
using CooleWebapp.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CooleWebapp.Application.Shop.Services
{
  internal class Products : IProducts
  {
    private readonly IProductDataAccess _productDataAccess;

    public Products(IProductDataAccess productDataAccess)
    {
      _productDataAccess = productDataAccess;
    }

    public async Task<byte[]> ReadProductImage(ulong productId, CancellationToken ct)
    {
      var imageData = await _productDataAccess.ReadProductImage(productId, ct);
      if (imageData is null)
        throw new ClientError(ErrorType.NotFound, "No product image available.");
      return imageData;
    }

    public async Task<GetProductsResponseModel> ReadProducts(
      GetShopProductsRequestModel getShopProductsRequestModel, 
      CancellationToken ct)
    {
      var res = await _productDataAccess
        .ReadProducts(
          new Page(getShopProductsRequestModel.PageIndex, getShopProductsRequestModel.PageSize),
          null,
          Core.Entities.ProductState.Available,
          SortDirection.ByNameAscending, 
          ct);

      return new(
        res.Pagination,
        res.Items.Select(p => new ProductResponseModel()
        {
          Description = p.Description,
          Id = p.Id,
          Name = p.Name,
          Price = p.Price,
          State = p.State
        }).ToImmutableArray());
    }
  }
}
