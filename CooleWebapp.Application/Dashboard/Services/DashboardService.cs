﻿using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;
using System.Collections.Immutable;

namespace CooleWebapp.Application.Dashboard.Services;

internal class DashboardService : IDashboardService
{
  private readonly IUserDataAccess _userDataAccess;
  private readonly IQueryPaginated _queryPaginated;
  private readonly IUserFilters _userFilters;
  public DashboardService(
    IUserDataAccess userDataAccess,
    IQueryPaginated queryPaginated,
    IUserFilters userFilters)
  {
    _userDataAccess = userDataAccess;
    _queryPaginated = queryPaginated;
    _userFilters = userFilters;
  }

  public async Task<GetRecentBuyersResponeModel> ReadRecentBuyers(
    uint pageIndex,
    uint pageSize,
    CancellationToken ct)
  {
    var query = (await _userFilters.FilterRegisteredUsersWithUserRole(_userDataAccess
      .GetAllUsers()))
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
    return new()
    {
      Buyers = new(
      paginated.Items.Select(item => new BuyerResponseModel()
      {
        CoolUserId = item.Id,
        Name = item.Name,
        Initials = item.Initials,
        Balance = item.Balance?.Value ?? 0,
        CanBuyOnFridge = (item.UserSettings?.BuyOnFridgePermission ?? BuyOnFridgePermission.NotPermitted) != BuyOnFridgePermission.NotPermitted
      }).ToImmutableArray(),
      paginated.Pagination)
    };
  }
}
