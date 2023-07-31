using CooleWebapp.Statistics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace CooleWebapp.Backend.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class StatisticsController : ControllerBase
{
  private readonly IStatisticsService _statisticsService;
  public StatisticsController(IStatisticsService statisticsService)
  {
    _statisticsService = statisticsService;
  }

  [Route("GetTotalPurchases")]
  [ProducesDefaultResponseType(typeof(GetTotalPurchasesResponseModel))]
  [HttpGet]
  public Task<GetTotalPurchasesResponseModel> GetTotalPurchases(CancellationToken ct)
  {
    return _statisticsService.GetTotalPurchases(ct);
  }
}
