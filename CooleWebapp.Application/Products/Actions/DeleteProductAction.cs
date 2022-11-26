using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using System.Reactive;

namespace CooleWebapp.Application.Products.Actions
{
  internal class DeleteProductAction : IBusinessAction<UInt64, Unit>
  {
    private readonly IProductDataAccess _productDataAccess;
    public DeleteProductAction(IProductDataAccess productDataAccess)
    {
      _productDataAccess = productDataAccess;
    }

    public async Task<Unit> Run(UInt64 dataIn, CancellationToken ct)
    {
      await _productDataAccess.DeleteProduct(dataIn, ct);
      return Unit.Default;
    }
  }
}
