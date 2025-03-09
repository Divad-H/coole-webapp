using CooleWebapp.Application.Products.Services;
using CooleWebapp.Backend.ErrorHandling;
using CooleWebapp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace CooleWebapp.Backend.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Roles.Administrator)]
[ApiController]
[Route("[controller]")]
public class AdminProductsController : ControllerBase
{
  private readonly IAdminProducts _adminProducts;
  public AdminProductsController(
    IAdminProducts adminProducts)
  {
    _adminProducts = adminProducts;
  }

  [Route("GetProducts")]
  [ProducesDefaultResponseType(typeof(GetProductsResponseModel))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpGet]
  public Task<GetProductsResponseModel> GetProducts(
    [FromQuery] GetProductsRequestModel data,
    CancellationToken ct)
  {
    return _adminProducts.ReadProducts(data, ct);
  }

  [Route("GetProductImage/{productId}")]
  [ProducesDefaultResponseType(typeof(FileResult))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpGet]
  public async Task<FileResult> GetProductImage([FromRoute] Int64 productId, CancellationToken ct)
  {
    var image = await _adminProducts.ReadProductImage(productId, ct);
    FileStreamResult file = new(new MemoryStream(image, false), "image/jpeg");
    file.FileDownloadName = "product-image.jpg";
    return file;
  }

  private async Task<byte[]> GetImage(IFormFile productImage, CancellationToken ct)
  {
    var imageBuffer = new byte[productImage.Length];
    using MemoryStream ms = new(imageBuffer);
    await productImage.CopyToAsync(ms, ct);
    return imageBuffer;
  }

  [Route("AddProduct")]
  [ProducesDefaultResponseType(typeof(Int64))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpPost]
  public async Task<Int64> AddProduct(
    IFormFile? productImage, 
    [FromForm] AddProductRequestModel product, 
    CancellationToken ct)
  {
    var image = productImage is null ? null : await GetImage(productImage, ct);
    var productId = await _adminProducts.CreateProduct(product, image, ct);
    return productId;
  }

  [Route("EditProduct")]
  [ProducesDefaultResponseType(typeof(Int64))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpPost]
  public async Task EditProduct(
    IFormFile? productImage, 
    [FromForm] EditProductRequestModel product, 
    CancellationToken ct)
  {
    var image = productImage is null ? null : await GetImage(productImage, ct);
    await _adminProducts.UpdateProduct(product, image, ct);
  }

  [Route("DeleteProduct/{productId}")]
  [ProducesDefaultResponseType(typeof(Int64))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpDelete]
  public Task DeleteProduct([FromRoute] Int64 productId, CancellationToken ct)
  {
    return _adminProducts.DeleteProduct(productId, ct);
  }
}
