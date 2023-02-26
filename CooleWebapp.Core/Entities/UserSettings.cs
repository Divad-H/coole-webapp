namespace CooleWebapp.Core.Entities;

public enum BuyOnFridgePermission
{
  NotPermitted,
  WithPinCode,
  AlwaysPermitted,
}

public record UserSettings
{
  public UInt64 Id { get; set; }
  public UInt64 CoolUserId { get; set; }
  public BuyOnFridgePermission BuyOnFridgePermission { get; set; }
  public string? BuyOnFridgePinCodeHash { get; set; }
}
