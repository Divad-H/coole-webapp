using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Database.Model;

namespace CooleWebapp.Database.Runners
{
  internal class RunnerFactory : IRunnerFactory
  {
    private readonly WebappDbContext _context;
    public RunnerFactory(WebappDbContext context) 
    {
      _context = context;
    }

    public IActionRunner<TIn, TOut> CreateTransactionRunner<TIn, TOut>(
      IBusinessAction<TIn, TOut> businessAction)
    {
      return new TransactionRunner<TIn, TOut>(businessAction, _context);
    }

    public IActionRunner<TIn1, TOut2> CreateTransaction2Runner<TIn1, TOut1, TIn2, TOut2>(
      IBusinessAction<TIn1, TOut1> businessAction1,
      Func<TOut1, TIn2> convert,
      IBusinessAction<TIn2, TOut2> businessAction2)
    {
      return new Transaction2Runner<TIn1, TOut1, TIn2, TOut2>(
        businessAction1, convert, businessAction2, _context);
    }
  }
}
