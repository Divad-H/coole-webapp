using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Users.Repository;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Statistics.Services;

internal class StatisticsService : IStatisticsService
{
  private readonly IAccountingDataAccess _accountingDataAccess;
  private readonly IUserDataAccess _userDataAccess;
  public StatisticsService(
    IAccountingDataAccess accountingDataAccess,
    IUserDataAccess userDataAccess)
  {
    _accountingDataAccess = accountingDataAccess;
    _userDataAccess = userDataAccess;
  }

  public async Task<GetTotalPurchasesResponseModel> GetTotalPurchases(CancellationToken ct)
  {
    var orderItems = await _accountingDataAccess.GetAllOrderItems(ct);
    return new() { 
      TotalItems = (uint)orderItems.Sum(i => (uint)i.Quantity), 
      TotalPrice = orderItems.Sum(i => i.Price * i.Quantity) 
    };
  }

  public async Task<IReadOnlyCollection<GetTopSpendersResponseModel>> GetTopSpenders(
    GetTopSpendersRequestModel getTopSpendersRequest, 
    CancellationToken ct)
  {
    var startDate = getTopSpendersRequest.TimePeriod switch
    {
      TimePeriod.Total => DateTime.MinValue,
      TimePeriod.OneMonth => DateTime.Now - TimeSpan.FromDays(30.437),
      TimePeriod.ThisMonth => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
      TimePeriod.OneYear => DateTime.Now - TimeSpan.FromDays(365.25),
      TimePeriod.ThisYear => new DateTime(DateTime.Now.Year, 1, 1),
      _ => DateTime.MinValue,
    };
    
    return await (await _accountingDataAccess.GetAllOrders(ct))
      .Where(o => o.Timestamp > startDate)
      .GroupJoin(
        await _accountingDataAccess.GetAllOrderItems(ct),
        o => o.Id, oi => oi.OrderId,
        (o, ois) => new { o.CoolUserId, Price = ois })
      .GroupBy(a => a.CoolUserId)
      .Select(g => new
      {
        CoolUserId = g.Key,
        Amount = g.SelectMany(i => i.Price).Sum(i => i.Quantity * i.Price)
      })
      .OrderByDescending(a => a.Amount)
      .Take((int)getTopSpendersRequest.NumberOfSpenders)
      .Join(
        _userDataAccess.GetAllUsers(),
         o => o.CoolUserId, u => u.Id,
        (spentAmount, u) => new GetTopSpendersResponseModel()
        {
          CoolUserId = u.Id,
          Name = u.Name,
          Initials = u.Initials,
          AmountSpent = spentAmount.Amount,
        })
      .ToArrayAsync(ct);
  }
}
