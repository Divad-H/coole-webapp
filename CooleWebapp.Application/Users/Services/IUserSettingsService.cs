using CooleWebapp.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Application.Users.Services
{
  public record GetSettingsResponseModel
  {
    public GetSettingsResponseModel(BuyOnFridgePermission buyOnFridgePermission)
    {
      BuyOnFridgePermission = buyOnFridgePermission;
    }

    [Required] public BuyOnFridgePermission BuyOnFridgePermission { get; set; }
  }

  public record UpdateSettingsRequestModel
  {
    [Required] public BuyOnFridgePermission BuyOnFridgePermission { get; set; }
    public string? BuyOnFridgePinCode { get; set; }
  }

  public interface IUserSettingsService
  {
    Task<GetSettingsResponseModel> ReadUserSettings(string webappUserId, CancellationToken ct);
    Task UpdateUserSettings(
      string webappUserId,
      UpdateSettingsRequestModel updateSettingsRequestModel,
      CancellationToken ct);
  }
}
