using CooleWebapp.Application.Accounting.Actions;
using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Products.Actions;
using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Application.Accounting.Services
{
  internal class UserAccount : IUserAccount
  {
    private readonly IAccountingDataAccess _accountingDataAccess;
    private readonly IUserDataAccess _userDataAccess;
    private readonly IRunnerFactory _runnerFactory;
    private readonly IFactory<AddBalanceAction> _addBalanceActionFactory;
    public UserAccount(
      IAccountingDataAccess balanceDataAccess,
      IUserDataAccess userDataAccess,
      IRunnerFactory runnerFactory,
      IFactory<AddBalanceAction> addBalanceActionFactory)
    {
      _accountingDataAccess = balanceDataAccess;
      _userDataAccess = userDataAccess;
      _runnerFactory = runnerFactory;
      _addBalanceActionFactory = addBalanceActionFactory;
    }

    public async Task<UserBalanceResponseModel> AddBalance(
      string webappUserId,
      decimal amount,
      CancellationToken ct)
    {
      var user = await _userDataAccess.FindUserByWebappUserId(webappUserId, ct)
        ?? throw new ClientError(ErrorType.NotFound, "No such user.");
      return new()
      {
        Balance = await _runnerFactory
        .CreateWriterRunner(_addBalanceActionFactory.Create())
        .Run(new(user.Id, amount), ct),
        UserName = user.Name
      };
    }

    public async Task<UserBalanceResponseModel> GetUserBalance(
      string webappUserId, CancellationToken ct)
    {
      var user = await _userDataAccess.FindUserByWebappUserId(webappUserId, ct)
        ?? throw new ClientError(ErrorType.NotFound, "No such user.");
      var balance = await _accountingDataAccess.GetBalance(user.Id, ct);
      return new()
      {
        Balance = balance.Value,
        UserName = user.Name,
      };
    }
  }
}
