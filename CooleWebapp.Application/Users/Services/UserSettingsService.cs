using CooleWebapp.Application.Users.Actions;
using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.ErrorHandling;
using CooleWebapp.Core.Utilities;

namespace CooleWebapp.Application.Users.Services
{
  internal sealed class UserSettingsService : IUserSettingsService
  {
    private readonly IUserSettingsDataAccess _userSettingsDataAccess;
    private readonly IUserDataAccess _userDataAccess;
    private readonly IRunnerFactory _runnerFactory;
    private readonly IFactory<UpdateUserSettingsAction> _updateUserSettingsActionFactory;
    public UserSettingsService(
      IUserSettingsDataAccess userSettingsDataAccess,
      IUserDataAccess userDataAccess,
      IRunnerFactory runnerFactory,
      IFactory<UpdateUserSettingsAction> updateUserSettingsActionFactory)
    {
      _userSettingsDataAccess = userSettingsDataAccess;
      _userDataAccess = userDataAccess;
      _runnerFactory = runnerFactory;
      _updateUserSettingsActionFactory = updateUserSettingsActionFactory;
    }

    public async Task<GetSettingsResponseModel> ReadUserSettings(
      string webappUserId, 
      CancellationToken ct)
    {
      var user = await _userDataAccess.FindUserByWebappUserId(webappUserId, ct)
        ?? throw new ClientError(ErrorType.NotFound, "No such user.");
      var userSettings = await _userSettingsDataAccess.GetUserSettings(user.Id, ct);
      return new(userSettings.BuyOnFridgePermission);
    }

    public async Task UpdateUserSettings(
      string webappUserId, 
      UpdateSettingsRequestModel updateSettingsRequestModel, 
      CancellationToken ct)
    {
      var user = await _userDataAccess.FindUserByWebappUserId(webappUserId, ct)
        ?? throw new ClientError(ErrorType.NotFound, "No such user.");
      await _runnerFactory
        .CreateWriterRunner(_updateUserSettingsActionFactory.Create())
        .Run(new(
          user.Id,
          updateSettingsRequestModel.BuyOnFridgePermission,
          updateSettingsRequestModel.BuyOnFridgePinCode), ct);
    }
  }
}
