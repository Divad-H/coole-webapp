namespace CooleWebapp.Core.BusinessActionRunners
{
  public interface IRunnerFactory
  {
    public IActionRunner<TIn, TOut> CreateTransactionRunner<TIn, TOut>(
      IBusinessAction<TIn, TOut> businessAction);
    public IActionRunner<TIn1, TOut2> CreateTransaction2Runner<TIn1, TOut1, TIn2, TOut2>(
      IBusinessAction<TIn1, TOut1> businessAction1,
      Func<TOut1, TIn2> convert,
      IBusinessAction<TIn2, TOut2> businessAction2);
  }
}
