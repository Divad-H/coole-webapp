using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;
using System.Collections.Immutable;

namespace CooleWebapp.Application.Dashboard.Services;

internal class DashboardService : IDashboardService
{
  private readonly IUserDataAccess _userDataAccess;
  private readonly IQueryPaginated _queryPaginated;
  public DashboardService(
    IUserDataAccess userDataAccess,
    IQueryPaginated queryPaginated)
  {
    _userDataAccess = userDataAccess;
    _queryPaginated = queryPaginated;
  }

  public async Task<GetRecentBuyersResponeModel> ReadRecentBuyers(
    uint pageIndex, 
    uint pageSize, 
    CancellationToken ct)
  {
    var query = _userDataAccess
      .GetAllUsers()
      .Select(u => new
      {
        u.Id,
        u.Name,
        u.Initials,
        u.Balance,
        u.UserSettings,
        Timestamp = u.Orders!.Select(o => o.Timestamp).OrderByDescending(t => t).FirstOrDefault()
      })
      .OrderByDescending(t => t.Timestamp);
    var paginated = await _queryPaginated.Execute(new(pageIndex, pageSize), query, ct);
    return new(new(
      paginated.Items.Select(item => new BuyerResponseModel(
        item.Id,
        item.Name,
        item.Initials,
        item.Balance?.Value ?? 0,
        (item.UserSettings?.BuyOnFridgePermission ?? BuyOnFridgePermission.NotPermitted) != BuyOnFridgePermission.NotPermitted))
          .ToImmutableArray(),
      paginated.Pagination));
  }
}
