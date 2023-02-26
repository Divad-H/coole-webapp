using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Application.Users.Services;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using System.Reactive;

namespace CooleWebapp.Application.Users.Actions;

internal sealed class UpdateUserSettingsAction : IBusinessAction<UpdateUserSettingsDto, Unit>
{
  private readonly IPinCodeHashing _pinCodeHashing;
  private readonly IUserSettingsDataAccess _userSettingsDataAccess;
  public UpdateUserSettingsAction(
    IPinCodeHashing pinCodeHashing,
    IUserSettingsDataAccess userSettingsDataAccess)
  {
    _pinCodeHashing = pinCodeHashing;
    _userSettingsDataAccess = userSettingsDataAccess;
  }

  public async Task<Unit> Run(UpdateUserSettingsDto dataIn, CancellationToken ct)
  {
    var settings = await _userSettingsDataAccess.GetUserSettings(dataIn.CoolUserId, ct);
    settings.BuyOnFridgePermission = dataIn.BuyOnFridgePermission;
    if (dataIn.BuyOnFridgePermission == BuyOnFridgePermission.WithPinCode)
    {
      if (settings.BuyOnFridgePinCodeHash is null && dataIn.BuyOnFridgePinCode is null)
        throw new ClientError(ErrorType.InvalidOperation, "A pin code must be provided.");
      if (dataIn.BuyOnFridgePinCode is not null)
      {
        if (!dataIn.BuyOnFridgePinCode.All(c => c >= '0' && c <= '9'))
          throw new ClientError(ErrorType.InvalidOperation, "A pin code must only contain digits.");
        if (dataIn.BuyOnFridgePinCode.Length < 4)
          throw new ClientError(ErrorType.InvalidOperation, "A pin code must be at least 4 digits long.");
        settings.BuyOnFridgePinCodeHash = _pinCodeHashing.GetHash(dataIn.BuyOnFridgePinCode);
      }
    }
    else
    {
      if (dataIn.BuyOnFridgePinCode is not null)
        throw new ClientError(ErrorType.InvalidOperation, "Unexpectedly received pin code.");
    }
    return Unit.Default;
  }
}
