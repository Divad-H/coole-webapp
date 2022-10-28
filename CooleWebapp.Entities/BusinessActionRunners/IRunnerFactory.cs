namespace CooleWebapp.Core.BusinessActionRunners
{
  public interface IRunnerFactory
  {
    public ITransactionRunner<TIn, TOut> CreateTransactionRunner<TIn, TOut>(
      IBusinessAction<TIn, TOut> businessAction);
  }
}
