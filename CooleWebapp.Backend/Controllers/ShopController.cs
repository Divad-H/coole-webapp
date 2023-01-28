using CooleWebapp.Application.Products.Services;
using CooleWebapp.Application.Shop.Services;
using CooleWebapp.Backend.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

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

    [Route("GetProductImage/{productId}")]
    [ProducesDefaultResponseType(typeof(FileResult))]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<FileResult> GetProductImage([FromRoute] UInt64 productId, CancellationToken ct)
    {
      var image = await _products.ReadProductImage(productId, ct);
      return File(image, "image/jpeg");
    }
  }
}
