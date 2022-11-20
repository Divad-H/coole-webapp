using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.BusinessActionRunners;

namespace CooleWebapp.Application.Products.Actions
{
  internal class CreateProductAction : IBusinessAction<CreateProductDto, UInt64>
  {
    public Task<UInt64> Run(CreateProductDto dataIn, CancellationToken ct)
    {
      throw new NotImplementedException();
    }
  }
}
