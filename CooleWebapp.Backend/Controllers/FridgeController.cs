using CooleWebapp.Backend.ErrorHandling;
using CooleWebapp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace CooleWebapp.Backend.Controllers;

public record GetBuyerResponseModel
{
  public UInt64 CoolUserId { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal Balance { get; set; }
  public BuyOnFridgePermission BuyOnFridgePermission { get; set; }
}


[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Roles.Fridge)]
[ApiController]
[Route("[controller]")]
public class FridgeController : ControllerBase
{
  [Route("GetBuyer")]
  [ProducesDefaultResponseType(typeof(GetBuyerResponseModel))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [HttpGet]
  public Task<GetBuyerResponseModel> GetBuyer(
    [FromQuery] UInt64 coolUserId,
    CancellationToken ct)
  {
    return Task.FromResult(new GetBuyerResponseModel()
    {
      CoolUserId = coolUserId,
      Balance = 10,
      BuyOnFridgePermission = BuyOnFridgePermission.WithPinCode,
      Name = "Herbert Lee"
    });
  }
}
