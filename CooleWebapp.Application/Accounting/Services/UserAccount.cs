using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Users;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Application.Accounting.Services
{
  internal class UserAccount : IUserAccount
  {
    private readonly IAccountingDataAccess _accountingDataAccess;
    private readonly IUserDataAccess _userDataAccess;
    public UserAccount(
      IAccountingDataAccess balanceDataAccess,
      IUserDataAccess userDataAccess)
    {
      _accountingDataAccess = balanceDataAccess;
      _userDataAccess = userDataAccess;
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
