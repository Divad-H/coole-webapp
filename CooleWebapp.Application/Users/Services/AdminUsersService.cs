using CooleWebapp.Application.Products.Actions;
using CooleWebapp.Application.Products.Services;
using CooleWebapp.Application.Users.Actions;
using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Application.Users.Services;

internal sealed class AdminUsersService : IAdminUsersService
{
  private readonly IUserDataAccess _userDataAccess;
  private readonly IQueryPaginated _queryPaginated;
  private readonly IRunnerFactory _runnerFactory;
  private readonly IFactory<UpdateUserAction> _updateUserActionFactory;
  private readonly IFactory<DeleteUserAction> _deleteUserActionFactory;
  public AdminUsersService(
    IUserDataAccess userDataAccess,
    IQueryPaginated queryPaginated,
    IRunnerFactory runnerFactory,
    IFactory<UpdateUserAction> updateUserActionFactory,
    IFactory<DeleteUserAction> deleteUserActionFactory)
  {
    _userDataAccess = userDataAccess;
    _queryPaginated = queryPaginated;
    _runnerFactory = runnerFactory;
    _updateUserActionFactory = updateUserActionFactory;
    _deleteUserActionFactory = deleteUserActionFactory;
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

  public Task DeleteUser(UInt64 userId, CancellationToken ct)
  {
    return _runnerFactory
      .CreateWriterRunner(_deleteUserActionFactory.Create())
      .Run(userId, ct);
  }

  public Task UpdateUser(EditUserRequestModel editUserRequestModel, CancellationToken ct)
  {
    return _runnerFactory
      .CreateWriterRunner(_updateUserActionFactory.Create())
      .Run(new()
      {
        Id = editUserRequestModel.UserId,
        Roles = editUserRequestModel.Roles ?? Array.Empty<UserRole>(),
      }, ct);
  }
}
