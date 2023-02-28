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
    if (dataIn.BuyOnFridgePermission != BuyOnFridgePermission.WithPinCode && !string.IsNullOrEmpty(dataIn.BuyOnFridgePinCode))
      throw new ClientError(ErrorType.InvalidOperation, "Must not send a pin code when no pin code is expected.");
    var settings = await _userSettingsDataAccess.GetUserSettings(dataIn.CoolUserId, ct);
    settings.BuyOnFridgePermission = dataIn.BuyOnFridgePermission;
    if (dataIn.BuyOnFridgePermission == BuyOnFridgePermission.WithPinCode)
    {
      var pinCode = dataIn.BuyOnFridgePinCode;
      if (string.IsNullOrEmpty(pinCode) && string.IsNullOrEmpty(settings.BuyOnFridgePinCodeHash))
        throw new ClientError(ErrorType.InvalidOperation, "Must send a pin code when a pin code is expected.");
      if (!string.IsNullOrEmpty(pinCode))
      {
        if (!pinCode.All(c => c >= '0' && c <= '9'))
          throw new ClientError(ErrorType.InvalidOperation, "Pin code must only contain digits.");
        if (pinCode.Length < 4)
          throw new ClientError(ErrorType.InvalidOperation, "Pin code must be at least 4 digits long.");
        settings.BuyOnFridgePinCodeHash = _pinCodeHashing.GetHash(pinCode);
      }
    }
    else
    {
      settings.BuyOnFridgePinCodeHash = null;
    }
    return Unit.Default;
  }
}
