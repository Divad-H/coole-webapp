using CooleWebapp.Core.Entities;

namespace CooleWebapp.Application.Users.Repository
{
  public interface IUserSettingsDataAccess
  {
    Task<UserSettings> GetUserSettings(UInt64 coolUserId, CancellationToken ct);
  }
}
