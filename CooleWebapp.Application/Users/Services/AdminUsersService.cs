using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Application.Users.Services;

internal sealed class AdminUsersService : IAdminUsersService
{
  private readonly IUserDataAccess _userDataAccess;
  private readonly IQueryPaginated _queryPaginated;
  public AdminUsersService(
    IUserDataAccess userDataAccess,
    IQueryPaginated queryPaginated)
  {
    _userDataAccess = userDataAccess;
    _queryPaginated = queryPaginated;
  }

  public async Task<GetUsersResponseModel> ReadUsers(
    GetUsersRequestModel getUsersRequest,
    CancellationToken ct)
  {
    var query = _userDataAccess.GetUsersWithRoles();
    if (getUsersRequest.SearchFilter != null)
    {
      query = query.Where(u => u.Name.ToLower()
        .Contains(getUsersRequest.SearchFilter.ToLower()));
    }
    if (getUsersRequest.SortDirection == Products.Services.SortDirection.ByNameDescending)
    {
      query = query.OrderByDescending(u => u.Name);
    }
    else
    {
      query = query.OrderBy(u => u.Name);
    }
    var users = await _queryPaginated.Execute(
      new Page(getUsersRequest.PageIndex, getUsersRequest.PageSize),
      query, ct);

    return new()
    {
      Pagination = users.Pagination,
      Users = users.Items.Select(u => new UserResponseModel()
      {
        Name = u.Name,
        Id = u.CoolUserId,
        Balance = u.Balance?.Value ?? 0,
        Email = u.Email ?? string.Empty,
        Roles = u.Roles.Select(r =>
        {
          if (Enum.TryParse<UserRole>(r, true, out var role))
            return role;
          else return UserRole.Unknown;
        }).ToArray()
      })
    };

  }
}
