using CooleWebapp.Application.Accounting.Repository;
using CooleWebapp.Application.Products.Repository;
using CooleWebapp.Application.Shop.Services;
using CooleWebapp.Application.Users.Repository;
using CooleWebapp.Application.Users.Services;
using CooleWebapp.Core.BusinessActionRunners;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using System.Reactive;

namespace CooleWebapp.Application.Shop.Actions
{
  internal class BuyProductsAsFridgeAction : IBusinessAction<BuyProductsAsFridgeDto, Unit>
  {
    private readonly IProductDataAccess _productDataAccess;
    private readonly IAccountingDataAccess _accountingDataAccess;
    private readonly IUserSettingsDataAccess _userSettingsDataAccess;
    private readonly IPinCodeHashing _pinCodeHashing;
    public BuyProductsAsFridgeAction(
      IProductDataAccess productDataAccess,
      IAccountingDataAccess balanceDataAccess,
      IUserSettingsDataAccess userSettingsDataAccess,
      IPinCodeHashing pinCodeHashing)
    {
      _productDataAccess = productDataAccess;
      _accountingDataAccess = balanceDataAccess;
      _userSettingsDataAccess = userSettingsDataAccess;
      _pinCodeHashing = pinCodeHashing;
    }

    public async Task<Unit> Run(BuyProductsAsFridgeDto dataIn, CancellationToken ct)
    {
      var settings = await _userSettingsDataAccess.GetUserSettings(dataIn.CoolUserId, ct);
      if (settings.BuyOnFridgePermission == BuyOnFridgePermission.NotPermitted)
        throw new ClientError(ErrorType.InvalidOperation, "The user did not allow to buy on fridge.");
      if (settings.BuyOnFridgePermission == BuyOnFridgePermission.WithPinCode)
      {
        if (settings.BuyOnFridgePinCodeHash is null)
          throw new InvalidOperationException("Inconsistend settings: No pin code hash was found in settings.");
        if (string.IsNullOrEmpty(dataIn.PinCode))
          throw new ClientError(ErrorType.InvalidOperation, "No pin code given.");
        if (!_pinCodeHashing.VerifyHash(dataIn.PinCode, settings.BuyOnFridgePinCodeHash))
          throw new ClientError(ErrorType.InvalidOperation, "Invalid pin code.");
        // Someone might want to brute force this..
      }

      return await BuyProductsAction.BuyProducts(
        dataIn.Products,
        dataIn.CoolUserId,
        _productDataAccess,
        _accountingDataAccess,
        ct);
    }
  }
}
