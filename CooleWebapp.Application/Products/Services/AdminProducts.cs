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

    public Task<ulong> CreateProduct(
      AddProductRequestModel addProductRequestModel,
      byte[]? productImage,
      CancellationToken ct)
    {
      throw new NotImplementedException();
    }

    public async Task<byte[]> ReadProductImage(UInt64 productId, CancellationToken ct)
    {
      throw new NotImplementedException();
    }

    public Task UpdateProduct(
      EditProductRequestModel editProductRequestModel,
      byte[]? productImage,
      CancellationToken ct)
    {
      throw new NotImplementedException();
    }

    public async Task<GetProductsResponseModel> ReadProducts(
      GetProductsRequestModel getProductsRequestModel, 
      CancellationToken ct)
    {
      throw new NotImplementedException();
    }

    public Task DeleteProduct(UInt64 productId, CancellationToken ct)
    {
      throw new NotImplementedException();
    }

  }
}
