using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.BusinessActionRunners;
using System.Reactive;

namespace CooleWebapp.Application.Products.Actions
{
  internal class UpdateProductAction : IBusinessAction<UpdateProductDto, Unit>
  {
    public Task<Unit> Run(UpdateProductDto dataIn, CancellationToken ct)
    {
      throw new NotImplementedException();
    }
  }
}
