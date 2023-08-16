using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users.Repository;

public record UserWithRoles
{
  public required UInt64 CoolUserId { get; init; }
  public required string Name { get; init; } = string.Empty;
  public Balance? Balance { get; init; }
  public required IReadOnlyCollection<string?> Roles { get; init; }
}

public interface IUserDataAccess
{
  Task<CoolUser> CreateUser(CoolUser user, CancellationToken ct);
  Task<CoolUser?> FindUserByWebappUserId(string webappUserId, CancellationToken ct);
  Task<CoolUser?> GetUser(UInt64 coolUserId, CancellationToken ct);
  IQueryable<CoolUser> GetAllUsers();
  IQueryable<UserWithRoles> GetUsersWithRoles();
}
