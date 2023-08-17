using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users.Services;

public record UpdateUserSettingsDto(
  UInt64 CoolUserId,
  BuyOnFridgePermission BuyOnFridgePermission,
  string? BuyOnFridgePinCode);


public record UpdateUserDto
{
  public required UInt64 Id { get; init; }
  public required IReadOnlyCollection<UserRole> Roles { get; init; }
}
