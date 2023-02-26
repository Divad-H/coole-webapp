using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Accounting.Services;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;

namespace CooleWebapp.Application.Accounting.Actions
{
  internal sealed class AddBalanceAction : IBusinessAction<AddBalanceDto, decimal>
  {
    private readonly IAccountingDataAccess _accountingDataAccess;
    public AddBalanceAction(
      IAccountingDataAccess accountingDataAccess)
    {
      _accountingDataAccess = accountingDataAccess;
    }

    public async Task<decimal> Run(AddBalanceDto dataIn, CancellationToken ct)
    {
      if (dataIn.Amount <= 0)
        throw new ClientError(ErrorType.InvalidOperation, "Amount must be positive.");
      if (dataIn.Amount > 100)
        throw new ClientError(ErrorType.InvalidOperation, "Please only add reasonable amounts.");
      var balance = await _accountingDataAccess.GetBalance(dataIn.CoolUserId, ct);
      balance.Version = Guid.NewGuid();
      balance.Value += dataIn.Amount;

      Deposit deposit = new()
      {
        Amount = dataIn.Amount,
        CoolUserId = dataIn.CoolUserId,
        Timestamp = DateTime.Now,
      };
      await _accountingDataAccess.CreateDeposít(deposit, ct);

      return balance.Value;
    }
  }
}
