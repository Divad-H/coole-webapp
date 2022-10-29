namespace CooleWebapp.Core.BusinessActionRunners;

public interface IRunnerFactory
{
  /// <summary>
  /// Creates a simple writer runner
  /// </summary>
  /// <param name="businessAction">The action</param>
  /// <returns>The newly created runner.</returns>
  public IActionRunner<TIn, TOut> CreateWriterRunner<TIn, TOut>(
    IBusinessAction<TIn, TOut> businessAction);
  /// <summary>
  /// Creates a runner that executes two actions before saving the changes.
  /// </summary>
  /// <param name="businessAction1">The first action</param>
  /// <param name="convert">Conversion function of the result of the first action to the input of the second action</param>
  /// <param name="businessAction2">The second action</param>
  /// <returns>The newly created runner.</returns>
  public IActionRunner<TIn1, TOut2> CreateWriter2Runner<TIn1, TOut1, TIn2, TOut2>(
    IBusinessAction<TIn1, TOut1> businessAction1,
    Func<TOut1, TIn2> convert,
    IBusinessAction<TIn2, TOut2> businessAction2);

  /// <summary>
  /// Creates a runner that executes two actions in a single transaction.
  /// </summary>
  /// <param name="businessAction1">The first action</param>
  /// <param name="convert">Conversion function of the result of the first action to the input of the second action</param>
  /// <param name="businessAction2">The second action</param>
  /// <returns>The newly created runner.</returns>
  public IActionRunner<TIn1, TOut2> CreateTransaction2Runner<TIn1, TOut1, TIn2, TOut2>(
    IBusinessAction<TIn1, TOut1> businessAction1,
    Func<TOut1, TIn2> convert,
    IBusinessAction<TIn2, TOut2> businessAction2);
}
