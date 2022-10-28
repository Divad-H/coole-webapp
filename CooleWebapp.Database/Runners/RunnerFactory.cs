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

    public ITransactionRunner<TIn, TOut> CreateTransactionRunner<TIn, TOut>(
      IBusinessAction<TIn, TOut> businessAction)
    {
      return new TransactionRunner<TIn, TOut>(businessAction, _context);
    }
  }
}
