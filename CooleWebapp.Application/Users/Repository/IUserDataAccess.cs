using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users.Repository;

public record UserWithRoles
{
  public required Int64 CoolUserId { get; init; }
  public required string Name { get; init; }
  public string? Email { get; init; }
  public Balance? Balance { get; init; }
  public required IReadOnlyCollection<string?> Roles { get; init; }
}

public interface IUserDataAccess
{
  Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct);
  Task DeleteUser(Int64 coolUserId, CancellationToken ct);
  Task<CoolUser?> FindUserByWebappUserId(string webappUserId, CancellationToken ct);
  Task<CoolUser?> GetUser(Int64 coolUserId, CancellationToken ct);
  Task SetUserRoles(Int64 coolUserId, IReadOnlyCollection<string> roles, CancellationToken ct);
  IQueryable<CoolUser> GetAllUsers();
  IQueryable<UserWithRoles> GetUsersWithRoles();
}
