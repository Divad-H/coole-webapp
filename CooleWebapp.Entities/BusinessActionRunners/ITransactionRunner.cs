namespace CooleWebapp.Core.BusinessActionRunners
{
  public interface ITransactionRunner<TIn, TOut>
  {
    public Task<TOut> Run(TIn dataIn, CancellationToken ct);
  }
}
