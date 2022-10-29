using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Database.Model;

namespace CooleWebapp.Database.Runners;

internal class Transaction2Runner<TIn1, TOut1, TIn2, TOut2> : IActionRunner<TIn1, TOut2>
{
  private readonly IBusinessAction<TIn1, TOut1> _businessAction1;
  private readonly Func<TOut1, TIn2> _intermediateFunc;
  private readonly IBusinessAction<TIn2, TOut2> _businessAction2;
  private readonly WebappDbContext _webappDbContext;
  public Transaction2Runner(
    IBusinessAction<TIn1, TOut1> businessAction1,
    Func<TOut1, TIn2> intermediateFunc,
    IBusinessAction<TIn2, TOut2> businessAction2,
  WebappDbContext webappDbContext)
  {
    _businessAction1 = businessAction1;
    _intermediateFunc = intermediateFunc;
    _businessAction2 = businessAction2;
    _webappDbContext = webappDbContext;
  }
  public async Task<TOut2> Run(TIn1 dataIn, CancellationToken ct)
  {
    using var transaction = await _webappDbContext.Database.BeginTransactionAsync(ct);
    var dataOut1 = await _businessAction1.Run(dataIn, ct).ConfigureAwait(false);
    await _webappDbContext.SaveChangesAsync(ct);
    var dataIn2 = _intermediateFunc(dataOut1);
    var res = await _businessAction2.Run(dataIn2, ct).ConfigureAwait(false);
    await _webappDbContext.SaveChangesAsync(ct);
    await transaction.CommitAsync(ct);
    return res;
  }
}
