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

  [Route("GetTopSpenders")]
  [ProducesDefaultResponseType(typeof(IReadOnlyCollection<GetTopSpendersResponseModel>))]
  [HttpGet]
  public Task<IReadOnlyCollection<GetTopSpendersResponseModel>> GetTopSpenders(
    [FromQuery] GetTopSpendersRequestModel getTopSpendersRequest,
    CancellationToken ct)
  {
    return _statisticsService.GetTopSpenders(getTopSpendersRequest, ct);
  }

  [Route("GetPurchasesPerTimeStatistics")]
  [ProducesDefaultResponseType(typeof(GetPurchasesPerTimeStatisticsResponseModel))]
  [HttpGet]
  public Task<GetPurchasesPerTimeStatisticsResponseModel> GetPurchasesPerTimeStatistics(
    [FromQuery] GetPurchasesPerTimeStatisticsRequestModel getPurchasesPerTimeStatisticsRequest,
    CancellationToken ct)
  {
    return _statisticsService.GetPurchasesPerTimeStatistics(getPurchasesPerTimeStatisticsRequest, ct);
  }

  [Route("GetProductStatistics")]
  [ProducesDefaultResponseType(typeof(IReadOnlyCollection<GetProductStatisticsResponseModel>))]
  [HttpGet]
  public Task<IReadOnlyCollection<GetProductStatisticsResponseModel>> GetProductStatistics(
    [FromQuery] GetProductStatisticsRequestModel getProductStatisticsRequest,
    CancellationToken ct)
  {
    return _statisticsService.GetProductStatistics(getProductStatisticsRequest, ct);
  }

  [Route("GetMostRecentPurchases")]
  [ProducesDefaultResponseType(typeof(IReadOnlyCollection<GetMostRecentPurchasesResponseModel>))]
  [HttpGet]
  public Task<IReadOnlyCollection<GetMostRecentPurchasesResponseModel>> GetMostRecentPurchases(
    [FromQuery] GetMostRecentPurchasesRequestModel getMostRecentPurchasesRequest,
    CancellationToken ct)
  {
    return _statisticsService.GetMostRecentPurchases(getMostRecentPurchasesRequest, ct);
  }
}
