using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Identity;
using CooleWebapp.Auth.Model;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Immutable;

namespace CooleWebapp.Backend.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AuthenticationController : ControllerBase
  {
    private readonly SignInManager<WebappUser> _signInManager;
    private readonly UserManager<WebappUser> _userManager;

    public AuthenticationController(
      SignInManager<WebappUser> signInManager,
      UserManager<WebappUser> userManager)
    {
      _signInManager = signInManager;
      _userManager = userManager;
    }

    [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
      var request = HttpContext.GetOpenIddictServerRequest();
      if (request is null)
        throw new ArgumentNullException(nameof(request));
      if (request.IsPasswordGrantType())
      {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
          await _userManager.FindByEmailAsync(request.Username);
        if (user == null)
        {
          var properties = new AuthenticationProperties(new Dictionary<string, string?>
          {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                  "The username/password couple is invalid."
          });

          return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Validate the username/password parameters and ensure the account is not locked out.
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
          var properties = new AuthenticationProperties(new Dictionary<string, string?>
          {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                  "The username/password couple is invalid."
          });

          return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Create the claims-based identity that will be used by OpenIddict to generate tokens.
        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role);

        // Add the claims that will be persisted in the tokens.
        identity.AddClaim(Claims.Subject, await _userManager.GetUserIdAsync(user))
                .AddClaim(Claims.Email, await _userManager.GetEmailAsync(user))
                .AddClaim(Claims.Name, await _userManager.GetUserNameAsync(user));
        foreach (var role in await _userManager.GetRolesAsync(user))
          identity.AddClaim(Claims.Role, role, Destinations.AccessToken);

        var principal = new ClaimsPrincipal(identity);

        // Note: in this sample, the granted scopes match the requested scope
        // but you may want to allow the user to uncheck specific scopes.
        // For that, simply restrict the list of scopes before calling SetScopes.
        principal.SetScopes(request.GetScopes());
        principal.SetDestinations(GetDestinations(request));

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
      }
      else if (request.IsRefreshTokenGrantType())
      {
        // Retrieve the claims principal stored in the authorization code/device code/refresh token.
        var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
        if (principal is null)
          throw new ArgumentNullException(nameof(principal));

        // Retrieve the user profile corresponding to the refresh token.
        var user = await _userManager.FindByIdAsync(principal.GetClaim(Claims.Subject));
        if (user == null)
        {
          var properties = new AuthenticationProperties(new Dictionary<string, string?>
          {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The refresh token is no longer valid."
          });

          return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Ensure the user is still allowed to sign in.
        if (!await _signInManager.CanSignInAsync(user))
        {
          var properties = new AuthenticationProperties(new Dictionary<string, string?>
          {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
          });

          return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        principal.SetDestinations(GetDestinations(request));

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
      }

      throw new NotImplementedException("The specified grant type is not implemented.");
    }

    private static ImmutableDictionary<string, string[]> GetDestinations(OpenIddictRequest request)
    {
      // Note: by default, claims are NOT automatically included in the access and identity tokens.
      // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
      // whether they should be included in access tokens, in identity tokens or in both.
      IEnumerable<string> getDests(string scope)
      {
        yield return Destinations.AccessToken;
        if (request.HasScope(scope) || true)
          yield return Destinations.IdentityToken;
      }

      IEnumerable<(string, string[])> getAllDests()
      {
        yield return (Claims.Name, getDests(Scopes.Profile).ToArray());
        yield return (Claims.Email, getDests(Scopes.Email).ToArray());
        yield return (Claims.Role, getDests(Scopes.Roles).ToArray());
      }

      return getAllDests().ToImmutableDictionary(kv => kv.Item1, kv => kv.Item2);
    }
  }
}
