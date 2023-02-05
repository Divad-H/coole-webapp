using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Database.Model;

namespace CooleWebapp.Database.Runners;

internal class WriteRunner<TIn, TOut> : IActionRunner<TIn, TOut>
{
  private readonly IBusinessAction<TIn, TOut> _businessAction;
  private readonly WebappDbContext _webappDbContext;
  public WriteRunner(
    IBusinessAction<TIn, TOut> businessAction,
    WebappDbContext webappDbContext)
  {
    _businessAction = businessAction;
    _webappDbContext = webappDbContext;
  }
  public async Task<TOut> Run(TIn dataIn, CancellationToken ct)
  {
    var res = await _businessAction.Run(dataIn, ct).ConfigureAwait(false);
    await _webappDbContext.SaveChangesAsync(ct);
    return res;
  }
}

