using CooleWebapp.Application.Products.Actions;
using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using CooleWebapp.Core.Utilities;
using System.Collections.Immutable;

namespace CooleWebapp.Application.Products.Services
{
  internal class AdminProducts : IAdminProducts
  {
    private readonly IProductDataAccess _productDataAccess;
    private readonly IRunnerFactory _runnerFactory;
    private readonly IFactory<CreateProductAction> _createProductActionFactory;
    private readonly IFactory<UpdateProductAction> _updateProductActionFactory;
    private readonly IFactory<DeleteProductAction> _deleteProductActionFactory;
    public AdminProducts(
      IProductDataAccess productDataAccess,
      IRunnerFactory runnerFactory,
      IFactory<CreateProductAction> createProductActionFactory,
      IFactory<UpdateProductAction> updateProductActionFactory,
      IFactory<DeleteProductAction> deleteProductActionFactory)
    {
      _productDataAccess = productDataAccess;
      _runnerFactory = runnerFactory;
      _createProductActionFactory = createProductActionFactory;
      _updateProductActionFactory = updateProductActionFactory;
      _deleteProductActionFactory = deleteProductActionFactory;
    }

    public Task<long> CreateProduct(
      AddProductRequestModel addProductRequestModel,
      byte[]? productImage,
      CancellationToken ct)
    {
      return _runnerFactory
        .CreateWriterRunner(_createProductActionFactory.Create())
        .Run(new()
        {
          Description = addProductRequestModel.Description,
          Name = addProductRequestModel.Name,
          Price = addProductRequestModel.Price,
          State = addProductRequestModel.State,
          Image = productImage,
        }, ct);
    }

    public async Task<byte[]> ReadProductImage(Int64 productId, CancellationToken ct)
    {
      var imageData = await _productDataAccess.ReadProductImage(productId, ct);
      if (imageData is null)
        throw new ClientError(ErrorType.NotFound, "No product image available.");
      return imageData;
    }

    public Task UpdateProduct(
      EditProductRequestModel editProductRequestModel,
      byte[]? productImage,
      CancellationToken ct)
    {
      return _runnerFactory
        .CreateWriterRunner(_updateProductActionFactory.Create())
        .Run(new()
        {
          Id = editProductRequestModel.ProductId,
          Description = editProductRequestModel.Description,
          Name = editProductRequestModel.Name,
          Price = editProductRequestModel.Price,
          State = editProductRequestModel.State,
          Image = editProductRequestModel.ImageUnchanged ? null : new() { Data = productImage },
        }, ct);
    }

    public async Task<GetProductsResponseModel> ReadProducts(
      GetProductsRequestModel getProductsRequestModel, 
      CancellationToken ct)
    {
      var res = await _productDataAccess
        .ReadProducts(
          new(getProductsRequestModel.PageIndex, getProductsRequestModel.PageSize),
          getProductsRequestModel.SearchFilter,
          getProductsRequestModel.ProductStateFilter,
          getProductsRequestModel.SortDirection, ct);
      
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

    public Task DeleteProduct(Int64 productId, CancellationToken ct)
    {
      return _runnerFactory
        .CreateWriterRunner(_deleteProductActionFactory.Create())
        .Run(productId, ct);
    }

  }
}
