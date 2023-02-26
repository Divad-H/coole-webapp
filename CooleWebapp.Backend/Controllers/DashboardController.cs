using CooleWebapp.Application.Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace CooleWebapp.Backend.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class DashboardController : ControllerBase
{
  private readonly IDashboardService _dashboardService;

  public DashboardController(IDashboardService dashboardService)
  {
    _dashboardService = dashboardService;
  }


  [Route("GetRecentBuyers")]
  [HttpGet]
  public Task<GetRecentBuyersResponeModel> GetRecentBuyers(
    UInt32 pageIndex, 
    UInt32 pageSize,
    CancellationToken ct)
  {
    return _dashboardService.ReadRecentBuyers(pageIndex, pageSize, ct);
  }
}
