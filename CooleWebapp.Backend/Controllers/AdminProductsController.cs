using CooleWebapp.Application.Products.Services;
using CooleWebapp.Backend.ErrorHandling;
using CooleWebapp.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace CooleWebapp.Backend.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Core.Entities.Roles.Administrator)]
[ApiController]
[Route("[controller]")]
public class AdminProductsController : ControllerBase
{
  [Route("GetProducts")]
  [ProducesDefaultResponseType(typeof(GetProductsResponseModel))]
  [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
  [HttpGet]
  public Task<GetProductsResponseModel> GetProducts(
    [FromQuery] GetProductsRequestModel data, 
    CancellationToken ct)
  {
    return Task.FromResult(new GetProductsResponseModel(
      new Pagination(12, new(data.PageIndex, data.PageSize == 0 ? 10 : data.PageSize)),
      Enumerable.Range(1, data.PageIndex == 0 ? 10 : 2)
        .Select(n => new Product()
        {
          Id = (UInt64)n,
          Description = $"Description {n}",
          Name = $"Product {n}",
          Price = 12.34M,
          State = Core.Entities.ProductState.Available
        })
      ));
  }
}
