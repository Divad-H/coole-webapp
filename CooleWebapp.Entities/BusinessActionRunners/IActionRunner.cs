namespace CooleWebapp.Core.BusinessActionRunners;

public interface IActionRunner<TIn, TOut>
{
  public Task<TOut> Run(TIn dataIn, CancellationToken ct);
}
