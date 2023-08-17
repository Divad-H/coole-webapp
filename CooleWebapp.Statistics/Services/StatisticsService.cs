using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Users.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data;

namespace CooleWebapp.Statistics.Services;

internal class StatisticsService : IStatisticsService
{
  private readonly IAccountingDataAccess _accountingDataAccess;
  private readonly IUserDataAccess _userDataAccess;
  private readonly IProductDataAccess _productDataAccess;
  public StatisticsService(
    IAccountingDataAccess accountingDataAccess,
    IUserDataAccess userDataAccess,
    IProductDataAccess productDataAccess)
  {
    _accountingDataAccess = accountingDataAccess;
    _userDataAccess = userDataAccess;
    _productDataAccess = productDataAccess;
  }

  public async Task<GetTotalPurchasesResponseModel> GetTotalPurchases(CancellationToken ct)
  {
    var orderItems = (await _accountingDataAccess.GetAllOrderItems(ct))
      .IgnoreQueryFilters();
    return new()
    {
      TotalItems = (uint)orderItems.Sum(i => (uint)i.Quantity),
      TotalPrice = orderItems.Sum(i => i.Price * i.Quantity)
    };
  }

  private static DateTime TimePeriodToDateTime(TimePeriod timePeriod)
  {
    return timePeriod switch
    {
      TimePeriod.Total => DateTime.MinValue,
      TimePeriod.OneMonth => DateTime.Now - TimeSpan.FromDays(30.437),
      TimePeriod.ThisMonth => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
      TimePeriod.OneYear => DateTime.Now - TimeSpan.FromDays(365.25),
      TimePeriod.ThisYear => new DateTime(DateTime.Now.Year, 1, 1),
      _ => DateTime.MinValue,
    };
  }

  public async Task<IReadOnlyCollection<GetTopSpendersResponseModel>> GetTopSpenders(
    GetTopSpendersRequestModel getTopSpendersRequest,
    CancellationToken ct)
  {
    var startDate = TimePeriodToDateTime(getTopSpendersRequest.TimePeriod);

    return await (await _accountingDataAccess.GetAllOrders(ct))
      .IgnoreQueryFilters()
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
        (spentAmount, u) => new
        {
          IsDeleted = u.IsDeleted,
          CoolUserId = u.Id,
          Name = u.Name,
          Initials = u.Initials,
          AmountSpent = spentAmount.Amount,
        })
      .AsAsyncEnumerable()
      .Select(u => new GetTopSpendersResponseModel()
      {
        CoolUserId = u.CoolUserId,
        Name = u.IsDeleted ? "Deleted" : u.Name,
        Initials = u.IsDeleted ? "???" : u.Initials,
        AmountSpent = u.AmountSpent,
      })
      .ToArrayAsync(ct);
  }

  public async Task<GetPurchasesPerTimeStatisticsResponseModel> GetPurchasesPerTimeStatistics(
    GetPurchasesPerTimeStatisticsRequestModel getPurchasesPerTimeStatisticsRequest,
    CancellationToken ct)
  {
    var orders = (await _accountingDataAccess.GetAllOrders(ct))
      .IgnoreQueryFilters();

    if (getPurchasesPerTimeStatisticsRequest.PurchaseStatisticsTimePeriod == PurchaseStatisticsTimePeriod.OneYear)
    {
      var startTime = DateTime.Now - TimeSpan.FromDays(365.25);
      orders = orders.Where(o => o.Timestamp > startTime);
    }

    var orderItems = await _accountingDataAccess.GetAllOrderItems(ct);
    if (getPurchasesPerTimeStatisticsRequest.ProductIdFilter is not null)
    {
      orderItems = orderItems.Where(o => o.ProductId == getPurchasesPerTimeStatisticsRequest.ProductIdFilter);
    }

    var res = await orders
     .GroupJoin(
        orderItems,
        o => o.Id, oi => oi.OrderId,
        (o, ois) => new { Date = new { o.Timestamp.Year, o.Timestamp.Month }, OrderItems = ois })
     .GroupBy(o => o.Date)
     .Select(g => new
     {
       Date = g.Key,
       NumberOfPurchases = g.SelectMany(i => i.OrderItems).Sum(i => i.Quantity)
     })
     .Where(g => g.NumberOfPurchases > 0)
     .OrderBy(o => o.Date.Year * 13 + o.Date.Month)
     .ToArrayAsync();

    if (res.Length == 0)
    {
      return new()
      {
        StartMonth = 0,
        StartYear = 0,
        NumberOfPurchases = Array.Empty<UInt32>()
      };
    }

    List<UInt32> numberOfPurchases = new();
    var startYear = (UInt32)res.First().Date.Year;
    var startMonth = (UInt32)res.First().Date.Month;
    var lastYear = (UInt32)res.Last().Date.Year;
    var lastMonth = (UInt32)res.Last().Date.Month;

    var year = startYear;
    var month = startMonth;
    int index = 0;
    while (year < lastYear || year == lastYear && month <= lastMonth)
    {
      if (res[index].Date.Year == year && res[index].Date.Month == month)
      {
        numberOfPurchases.Add((UInt32)res[index].NumberOfPurchases);
        ++index;
      }
      else
      {
        numberOfPurchases.Add(0);
      }
      ++month;
      if (month > 12)
      {
        month = 1;
        ++year;
      }
    }

    return new GetPurchasesPerTimeStatisticsResponseModel()
    {
      StartYear = startYear,
      StartMonth = startMonth,
      NumberOfPurchases = numberOfPurchases,
    };
  }

  public async Task<IReadOnlyCollection<GetProductStatisticsResponseModel>> GetProductStatistics(
    GetProductStatisticsRequestModel getProductStatisticsRequest,
    CancellationToken ct)
  {
    var startDate = TimePeriodToDateTime(getProductStatisticsRequest.TimePeriod);

    return await (await _accountingDataAccess.GetAllOrders(ct))
      .IgnoreQueryFilters()
      .Where(o => o.Timestamp > startDate)
      .GroupJoin(
        await _accountingDataAccess.GetAllOrderItems(ct),
        o => o.Id, oi => oi.OrderId,
        (o, ois) => ois)
      .SelectMany(ois => ois)
      .GroupBy(oi => oi.ProductId)
      .Select(oi => new
      {
        ProductId = oi.Key,
        NumberOfPurchases = oi.Sum(i => i.Quantity)
      })
      .Join(
        await _productDataAccess.ReadAllProducts(ct),
        g => g.ProductId, p => p.Id,
        (g, p) => new GetProductStatisticsResponseModel
        {
          ProductId = g.ProductId,
          ProductName = p.Name,
          NumberOfPurchases = g.NumberOfPurchases
        })
      .ToArrayAsync(ct);
  }

  public async Task<IReadOnlyCollection<GetMostRecentPurchasesResponseModel>> GetMostRecentPurchases(
    GetMostRecentPurchasesRequestModel getMostRecentPurchasesRequest,
    CancellationToken ct)
  {
    return await (await _accountingDataAccess.GetAllOrders(ct))
      .IgnoreQueryFilters()
      .OrderByDescending(o => o.Timestamp)
      .Take(getMostRecentPurchasesRequest.MaxNumberOfPurchases)
      .Join(
        await _accountingDataAccess.GetAllOrderItems(ct),
        o => o.Id, oi => oi.OrderId,
        (order, orderItem) => new
        {
          order.CoolUserId,
          orderItem.Quantity,
          orderItem.Price,
          orderItem.ProductId,
        }
      )
      .Take(getMostRecentPurchasesRequest.MaxNumberOfPurchases)
      .Join(
        _userDataAccess.GetAllUsers(),
        o => o.CoolUserId, u => u.Id,
        (o, user) => new
        {
          o.CoolUserId,
          o.Quantity,
          Price = o.Price * o.Quantity,
          o.ProductId,
          user.Name,
          user.Initials,
          user.IsDeleted
        }
      )
      .Join(
        await _productDataAccess.ReadAllProducts(ct),
        o => o.ProductId, p => p.Id,
        (o, product) => new
        {
          o.IsDeleted,
          BuyerCoolUserId = o.CoolUserId,
          BuyerInitials = o.Initials,
          BuyerName = o.Name,
          Price = o.Price,
          ProductId = o.ProductId,
          ProductName = product.Name,
          Quantity = o.Quantity,
        }
      )
      .AsAsyncEnumerable()
      .Select(o => new GetMostRecentPurchasesResponseModel()
      {
        BuyerCoolUserId = o.BuyerCoolUserId,
        BuyerInitials = o.IsDeleted ? "???" : o.BuyerInitials,
        BuyerName = o.IsDeleted ? "Deleted" : o.BuyerName,
        Price = o.Price,
        ProductId = o.ProductId,
        ProductName = o.ProductName, 
        Quantity = o.Quantity,
      })
      .ToArrayAsync(ct);
  }
}
