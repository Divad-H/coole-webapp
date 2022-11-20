using CooleWebapp.Core.BusinessActionRunners;
using System.Reactive;

namespace CooleWebapp.Application.Products.Actions
{
  internal class DeleteProductAction : IBusinessAction<UInt64, Unit>
  {
    public Task<Unit> Run(ulong dataIn, CancellationToken ct)
    {
      throw new NotImplementedException();
    }
  }
}
