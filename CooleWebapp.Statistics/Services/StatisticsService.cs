using CooleWebapp.Application.Accounting.Repository;

namespace CooleWebapp.Statistics.Services;

internal class StatisticsService : IStatisticsService
{
  private readonly IAccountingDataAccess _accountingDataAccess;
  public StatisticsService(IAccountingDataAccess accountingDataAccess)
  {
    _accountingDataAccess = accountingDataAccess;
  }

  public async Task<GetTotalPurchasesResponseModel> GetTotalPurchases(CancellationToken ct)
  {
    var orderItems = await _accountingDataAccess.GetAllOrderItems(ct);
    return new() { 
      TotalItems = (uint)orderItems.Sum(i => (uint)i.Quantity), 
      TotalPrice = orderItems.Sum(i => i.Price * i.Quantity) 
    };
  }
}
