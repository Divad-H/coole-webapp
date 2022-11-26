using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.BusinessActionRunners;
using System.Reactive;

namespace CooleWebapp.Application.Products.Actions
{
  internal class UpdateProductAction : IBusinessAction<UpdateProductDto, Unit>
  {
    private readonly IProductDataAccess _productDataAccess;
    public UpdateProductAction(IProductDataAccess productDataAccess)
    {
      _productDataAccess = productDataAccess;
    }
    public async Task<Unit> Run(UpdateProductDto dataIn, CancellationToken ct)
    {
      if (dataIn.Image is not null)
        await _productDataAccess.DeleteProductImage(dataIn.Id, ct);
      await _productDataAccess.UpdateProduct(new()
      {
        Id = dataIn.Id,
        Name = dataIn.Name,
        Description = dataIn.Description,
        Price = dataIn.Price,
        State = dataIn.State,
        ProductImage = dataIn.Image?.Data is null ? null : new() { Data = dataIn.Image.Data },
      }, ct);
      return Unit.Default;
    }
  }
}
