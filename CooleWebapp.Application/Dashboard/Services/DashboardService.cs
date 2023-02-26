using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;

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
        Timestamp = u.Deposits!
        .Select(d => d.Timestamp)
        .OrderByDescending(d => d)
        .FirstOrDefault()
      })
      .OrderByDescending(u => u.Timestamp);
    var res = await _queryPaginated.Execute(new Page(pageIndex, pageSize), query, ct);
    return new(new(res.Items.Select(d => new BuyerResponseModel(
      d.Id,
      d.Name,
      d.Initials,
      d.Balance?.Value ?? 0,
      (d.UserSettings?.BuyOnFridgePermission ?? BuyOnFridgePermission.NotPermitted) 
        != BuyOnFridgePermission.NotPermitted))
        .ToArray(),
      res.Pagination));
  }
}
