using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users.Services;

public record UpdateUserSettingsDto(
  Int64 CoolUserId,
  BuyOnFridgePermission BuyOnFridgePermission,
  string? BuyOnFridgePinCode);


public record UpdateUserDto
{
  public required Int64 Id { get; init; }
  public required IReadOnlyCollection<UserRole> Roles { get; init; }
}
