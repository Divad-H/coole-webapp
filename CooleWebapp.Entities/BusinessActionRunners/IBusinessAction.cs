namespace CooleWebapp.Core.BusinessActionRunners
{
  public interface IBusinessAction<TIn, TOut>
  {
    Task<TOut> Run(TIn dataIn, CancellationToken ct);
  }
}
