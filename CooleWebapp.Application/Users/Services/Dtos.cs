using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users.Services
{
  public record UpdateUserSettingsDto(
    UInt64 CoolUserId,
    BuyOnFridgePermission BuyOnFridgePermission,
    string? BuyOnFridgePinCode);
}
