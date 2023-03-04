using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.Entities;
using CooleWebapp.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace CooleWebapp.Database.Repository
{
  internal sealed class UserSettingsDataAccess : IUserSettingsDataAccess
  {
    private readonly WebappDbContext _dbContext;
    public UserSettingsDataAccess(WebappDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<UserSettings> GetUserSettings(ulong coolUserId, CancellationToken ct)
    {
      var settings = await _dbContext.UserSettings.FirstOrDefaultAsync(s => s.CoolUserId == coolUserId, ct);
      if (settings is null)
      {
        settings = new()
        {
          BuyOnFridgePermission = BuyOnFridgePermission.NotPermitted,
          CoolUserId = coolUserId
        };
        _dbContext.Attach(settings);
      }
      return settings;
    }
  }
}
