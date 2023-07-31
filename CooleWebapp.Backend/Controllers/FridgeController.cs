using CooleWebapp.Application.Accounting.Services;
using CooleWebapp.Backend.ErrorHandling;
using CooleWebapp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using CooleWebapp.Application.Dashboard.Services;
using CooleWebapp.Application.Shop.Services;

namespace CooleWebapp.Backend.Controllers;


[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Roles.Fridge)]
[ApiController]
[Route("[controller]")]
public class FridgeController : ControllerBase
{
  private readonly IUserAccount _userAccount;
  private readonly IDashboardService _dashboardService;
  private readonly IProducts _products;

  public FridgeController(
    IUserAccount userAccount,
    IDashboardService dashboardService,
    IProducts products)
  {
    _userAccount = userAccount;
    _dashboardService = dashboardService;
    _products = products;
  }

  [Route("GetBuyer")]
  [ProducesDefaultResponseType(typeof(GetBuyerResponseModel))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [HttpGet]
  public Task<GetBuyerResponseModel> GetBuyer(
    [FromQuery] UInt64 coolUserId,
    CancellationToken ct)
  {
    return _dashboardService.ReadBuyerDetails(coolUserId, ct);
  }

  [Route("AddBalance")]
  [ProducesDefaultResponseType(typeof(UserBalanceResponseModel))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status403Forbidden)]
  [HttpPost]
  public Task<UserBalanceResponseModel> AddBalance(
    AddBalanceRequestModel addBalanceRequestModel,
    UInt64 coolUserId,
    CancellationToken ct)
  {
    return _userAccount.AddBalance(coolUserId, addBalanceRequestModel.Amount, ct);
  }

  [Route("BuyProducts")]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpPost]
  public Task BuyProducts(
    BuyProductsAsFridgeRequestModel buyProductsRequestModel, 
    CancellationToken ct)
  {
    return _products.BuyProductsAsFridge(
      new()
      {
        Products = buyProductsRequestModel.Products,
        CoolUserId = buyProductsRequestModel.CoolUserId,
        PinCode = buyProductsRequestModel.PinCode
      },
      ct);
  }
}
