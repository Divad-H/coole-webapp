using CooleWebapp.Application.Products.Services;
using CooleWebapp.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Users.Services;

public enum UserRole
{
  User,
  Administrator,
  Fridge,
  Registered,
  Unknown,
}

public record UserResponseModel
{
  [Required] public required UInt64 Id { get; init; }
  [MaxLength(256), Required] public required string Name { get; init; }
  [Required] public required decimal Balance { get; init; }
  [Required] public required IReadOnlyCollection<UserRole> Roles { get; init; }
}

public record GetUsersResponseModel
{
  [Required] public required Pagination Pagination { get; init; }
  [Required] public required IEnumerable<UserResponseModel> Users { get; init; }
}

public record GetUsersRequestModel
{
  public required UInt32 PageIndex { get; init; }
  public required UInt32 PageSize { get; init; }
  public string? SearchFilter { get; init; }
  public required SortDirection SortDirection { get; init; }
}

public interface IAdminUsersService
{
  Task<GetUsersResponseModel> ReadUsers(GetUsersRequestModel getUsersRequest, CancellationToken ct);
}
