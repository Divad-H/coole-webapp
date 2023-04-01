using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Products.Services;
using CooleWebapp.Application.Shop.Actions;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using CooleWebapp.Core.Utilities;
using System.Collections.Immutable;

namespace CooleWebapp.Application.Shop.Services
{
  internal class Products : IProducts
  {
    private readonly IProductDataAccess _productDataAccess;
    private readonly IRunnerFactory _runnerFactory;
    private readonly IFactory<BuyProductsAction> _buyProductsActionFactory;
    private readonly IFactory<BuyProductsAsFridgeAction> _buyProductsAsFridgeActionFactory;

    public Products(
      IProductDataAccess productDataAccess,
      IRunnerFactory runnerFactory,
      IFactory<BuyProductsAction> buyProductsActionFactory,
      IFactory<BuyProductsAsFridgeAction> buyProductsAsFridgeActionFactory)
    {
      _productDataAccess = productDataAccess;
      _runnerFactory = runnerFactory;
      _buyProductsActionFactory = buyProductsActionFactory;
      _buyProductsAsFridgeActionFactory = buyProductsAsFridgeActionFactory;
    }

    public async Task<byte[]> ReadProductImage(ulong productId, CancellationToken ct)
    {
      return await _productDataAccess.ReadProductImage(productId, ct)
        ?? throw new ClientError(ErrorType.NotFound, "No product image available.");
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

      return new()
      {
        Pagination = res.Pagination,
        Products = res.Items.Select(p => new ProductResponseModel()
        {
          Description = p.Description,
          Id = p.Id,
          Name = p.Name,
          Price = p.Price,
          State = p.State
        }).ToImmutableArray()
      };
    }

    public async Task BuyProducts(
      BuyProductsDto buyProductsDto, 
      CancellationToken ct)
    {
      await _runnerFactory
        .CreateWriterRunner(_buyProductsActionFactory.Create())
        .Run(buyProductsDto, ct);
    }

    public async Task BuyProductsAsFridge(
      BuyProductsAsFridgeDto buyProductsAsFridgeDto, 
      CancellationToken ct)
    {
      await _runnerFactory
        .CreateWriterRunner(_buyProductsAsFridgeActionFactory.Create())
        .Run(buyProductsAsFridgeDto, ct);
    }
  }
}
