﻿using CooleWebapp.Application.Users.Services;
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
  [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = Roles.User)]
  [ApiController]
  [Route("[controller]")]
  public class UserSettingsController : ControllerBase
  {
    private readonly IUserSettingsService _userSettingsService;

    public UserSettingsController(
      IUserSettingsService userSettingsService)
    {
      _userSettingsService = userSettingsService;
    }

    [Route("GetSettings")]
    [ProducesDefaultResponseType(typeof(GetSettingsResponseModel))]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public Task<GetSettingsResponseModel> GetSettings(CancellationToken ct)
    {
      var userId = User.FindFirstValue(Claims.Subject)
        ?? throw new ClientError(ErrorType.NotFound, "User not found.");
      return _userSettingsService.ReadUserSettings(userId, ct);
    }

    [Route("UpdateSettings")]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [HttpPut]
    public Task UpdateSettings(
      UpdateSettingsRequestModel updateSettingsRequestModel, 
      CancellationToken ct)
    {
      var userId = User.FindFirstValue(Claims.Subject)
        ?? throw new ClientError(ErrorType.NotFound, "User not found.");
      return _userSettingsService.UpdateUserSettings(userId, updateSettingsRequestModel, ct);
    }
  }
}
