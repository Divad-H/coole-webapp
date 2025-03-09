using CooleWebapp.Application.Products.Services;
using CooleWebapp.Application.Shop.Services;
using CooleWebapp.Backend.ErrorHandling;
using CooleWebapp.Core.Entities;
using CooleWebapp.Core.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace CooleWebapp.Backend.Controllers
{
  [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
  [ApiController]
  [Route("[controller]")]
  public class ShopController : ControllerBase
  {
    private readonly IProducts _products;

    public ShopController(IProducts products)
    {
      _products = products;
    }

    [Route("GetProducts")]
    public Task<GetProductsResponseModel> GetProducts(
      GetShopProductsRequestModel getShopProductsRequestModel,
      CancellationToken ct)
    {
      return _products.ReadProducts(getShopProductsRequestModel, ct);
    }

    [Route("GetShortProducts")]
    public Task<IReadOnlyCollection<ShortProductResponseModel>> GetShortProducts(CancellationToken ct)
    {
      return _products.ReadShortProducts(ct);
    }

    [Route("GetProductImage/{productId}")]
    [ProducesDefaultResponseType(typeof(FileResult))]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<FileResult> GetProductImage([FromRoute] Int64 productId, CancellationToken ct)
    {
      var image = await _products.ReadProductImage(productId, ct);
      return File(image, "image/jpeg");
    }

    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Roles.User)]
    [Route("BuyProducts")]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public Task BuyProducts(BuyProductsRequestModel buyProductsRequestModel, CancellationToken ct)
    {
      var userId = User.FindFirstValue(Claims.Subject)
        ?? throw new ClientError(ErrorType.NotFound, "User not found.");
      return _products.BuyProducts(
        new() { Products = buyProductsRequestModel.Products, WebappUserId = userId },
        ct);
    }
  }
}
